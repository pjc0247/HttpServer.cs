using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.WebSocket.Middlewares
{
    public class Close : WebSocketMiddleware
    {
        public override WebResponse OnPreprocess(Session session, WebSocketRequest request)
        {
            if (request.opcode != OpCode.Close) return null;
            if (session.state != SessionState.Opened) return null;

            var close = new WebSocketResponse();
            close.opcode = OpCode.Close;
            close.content = request.content;

            throw new CloseSessionException(close);
        }
    }
}
