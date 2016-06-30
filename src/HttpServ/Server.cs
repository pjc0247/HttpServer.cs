using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace HttpServ
{
    using Http;
    using System.Security.Cryptography.X509Certificates;

    public class Server
    {
        internal IAdaptor adaptor { get; private set; }
        private List<IMiddleware> middlewares { get; set; }
        private TcpListener listener { get; set; }
        private Thread acceptWorker { get; set; }

        private long nextSessionId { get; set; }

        public bool isRunning { get; private set; }
        public bool isHttps { get; private set; }
        public X509Certificate2 cert { get; private set; }

        internal Server(IAdaptor adaptor, X509Certificate2 cert)
        {
            this.adaptor = adaptor;
            this.middlewares = new List<IMiddleware>();

            if (cert != null)
            {
                this.isHttps = true;
                this.cert = cert;
            }
        }

        public void Open(int port)
        {
            var host = IPAddress.Parse("0.0.0.0");

            listener = new TcpListener(host, port);
            listener.Start();
            acceptWorker = new Thread(AcceptWorker);
            acceptWorker.Start();

            isRunning = true;
        }

        public void AddMiddleware<Type>()
            where Type : IMiddleware, new()
        {
            if (isRunning)
                throw new InvalidOperationException("server already opend");

            middlewares.Add(new Type());
        }

        internal WebResponse ProcessRequest(Session session, WebRequest request)
        {
            foreach (var middleware in middlewares)
            {
                var response = middleware.OnPreprocess(session, request);

                if (response != null)
                    return response;
            }

            return adaptor.OnRequest(session, request);
        }
        internal void Postprocess(Session session, WebRequest request, WebResponse response)
        {
            foreach (var middleware in middlewares)
            {
                middleware.OnPostprocess(session,
                    request, response);
            }
        }

        private void AcceptWorker()
        {
            if (listener == null)
                throw new InvalidOperationException("listener -> null");

            while (true)
            {
                var sock = listener.AcceptTcpClient();

                try {
                    CreateSession(sock);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }

                nextSessionId++;
            }
        }
        private void CreateSession(TcpClient sock)
        {
            System.IO.Stream stream = null;

            if (isHttps)
            {
                var sslStream = new System.Net.Security.SslStream(sock.GetStream(), false);
                sslStream.AuthenticateAsServer(
                    cert, false,
                    SslProtocols.Tls12 | SslProtocols.Tls11
                    , false);
                stream = sslStream;
            }
            else
            {
                stream = sock.GetStream();
            }

            var session = new Session(sock, stream, this, nextSessionId);
            session.Open();
        }
    }
}
