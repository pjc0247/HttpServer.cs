using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.WebSocket
{
    public class WebSocketCloseResponse : WebSocketResponse
    {
        public WebSocketCloseResponse(ushort code, string reason)
        {
            this.opcode = OpCode.Close;

            SetContent(
                BitConverter.GetBytes(code)
                .Concat(Encoding.UTF8.GetBytes(reason))
                .ToArray());
        }
        public WebSocketCloseResponse(StatusCode code, string reason)
        {
            this.opcode = OpCode.Close;

            SetContent(
                BitConverter.GetBytes((int)code)
                .Concat(Encoding.UTF8.GetBytes(reason))
                .ToArray());
        }
    }
}
