using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.WebSocket
{
    public enum OpCode
    {
        Text = 1,
        Binary = 2,

        Ping = 9,
        Pong = 10
    }
}
