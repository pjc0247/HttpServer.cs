using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.WebSocket
{
    internal static class RequestValidator
    {
        public static void Validate(this WebSocketHeader header)
        {
            if (header.mask == false)
                throw new ArgumentException("request packet must be masked");
            if (header.opcode > (int)OpCode.Pong)
                throw new ArgumentException("unknown opcode");
        }
    }
}
