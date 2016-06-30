using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HttpServ;
using HttpServ.Simple;
using HttpServ.StaticFileAdaptor;
using System.Security.Cryptography.X509Certificates;

namespace Testbed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            X509Certificate2 cert = new X509Certificate2("server.pfx", "instant");
            var serv = ServerFactory.CreateHttps<SimpleAdaptor>(cert);
            serv.AddMiddleware<HttpServ.WebSocket.Middlewares.WebSocketHandshaker>();
            serv.AddMiddleware<HttpServ.WebSocket.Middlewares.PingPong>();
            serv.AddMiddleware<HttpServ.WebSocket.Middlewares.Close>();
            serv.Open(443);

            Console.Read();
        }
    }
}
