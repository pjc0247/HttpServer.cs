using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class ErrorCode
    {
        public static readonly int NoError = 0;
        public static readonly int ProtocolError = 1;
        public static readonly int InternalError = 2;
        public static readonly int FlowcontrolError = 3;
        public static readonly int SettingsTimeout = 4;
        public static readonly int StreamClosed = 5;
    }
}
