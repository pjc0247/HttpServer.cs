using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HttpServ;
using HttpServ.Simple;

namespace Testbed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serv = ServerFactory.Create<SimpleAdaptor>();
            serv.AddMiddleware<HttpServ.WebSocket.Middlewares.WebSocketHandshaker>();
            serv.AddMiddleware<HttpServ.WebSocket.Middlewares.PingPong>();
            serv.Open(9916);

            Console.Read();
        }
    }
}
