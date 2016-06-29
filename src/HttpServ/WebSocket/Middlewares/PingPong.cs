using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.WebSocket.Middlewares
{
    public class PingPong : WebSocketMiddleware
    {
        public override WebResponse OnPreprocess(Session session, WebSocketRequest request)
        {
            if (request.opcode != OpCode.Ping) return null;

            var pong = new WebSocketResponse();
            pong.opcode = OpCode.Pong;
            pong.content = request.content;
            return pong;
        }
    }
}
