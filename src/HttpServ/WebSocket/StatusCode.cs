using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.WebSocket
{
    public enum StatusCode
    {
        Normal = 1000,
        Away = 1001,
        ProtocolError = 1002,
        UnsupportedData = 1003,
        Undefined = 1004,
        NoStatus = 1005,
        Abnormal = 1006,
        PolicyViolation = 1008,
        TooBig = 1009,
        MadatoryExtension = 1010,
        ServerError = 1011,
        TlsHandshakeFailure = 1015
    }
}
