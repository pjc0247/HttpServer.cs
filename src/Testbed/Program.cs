using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HttpServ;
using HttpServ.Simple;
using HttpServ.StaticFileAdaptor;
using System.Security.Cryptography.X509Certificates;

using HttpServ.WebSocket;

namespace Testbed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cert = new X509Certificate2("server.pfx", "instant");
            var serv = ServerFactory.CreateHttp<HttpServ.CGI.CgiAdaptor>();
            serv.AddMiddleware<HttpServ.Http2.Middlewares.Http2Handshake>();
            serv.AddMiddleware<HttpServ.Http2.Middlewares.PrefaceHandshake>();
            serv.AddMiddleware<HttpServ.Http2.Middlewares.Setting>();
            serv.AddMiddleware<HttpServ.Http2.Middlewares.PingPong>();
            serv.EnableWebSocket();
            serv.Open(9916);

            Console.Read();
        }
    }
}
