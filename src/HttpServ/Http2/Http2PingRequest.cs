using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class Http2PingRequest : Http2Request
    {
        public byte[] opaqueData { get; set; }

        public Http2PingRequest()
        {
            opaqueData = new byte[2] { 0, 0 };
        }
    }
}
