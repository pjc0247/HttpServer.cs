using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.WebSocket
{
    public class WebSocketRequest : WebRequest
    {
        public OpCode opcode { get; set; }
    }
}
