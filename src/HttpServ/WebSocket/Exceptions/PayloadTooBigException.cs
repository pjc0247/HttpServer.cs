using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.WebSocket.Exceptions
{
    public class PayloadTooBigException : CloseSessionException
    {
        public PayloadTooBigException()
        {
            response = new WebSocketCloseResponse(
                StatusCode.TooBig, "too big");
        }
    }
}
