using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.WebSocket.Exceptions
{
    /// <summary>
    /// 웹소켓을 닫는다.
    /// </summary>
    public class CloseWebSocketException : CloseSessionException
    {
        public CloseWebSocketException(ushort code=1000, string reason="")
        {
            response = new WebSocketCloseResponse(code, reason);
        }
        public CloseWebSocketException(StatusCode code = StatusCode.Normal, string reason = "")
        {
            response = new WebSocketCloseResponse((ushort)code, reason);
        }
    }
}
