using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.WebSocket
{
    public static class WebSocketExt
    {
		public static void EnableWebSocket(this Server server)
        {
            server.AddMiddleware<Middlewares.WebSocketHandshaker>();
            server.AddMiddleware<Middlewares.Close>();
            server.AddMiddleware<Middlewares.PingPong>();
        }
    }
}
