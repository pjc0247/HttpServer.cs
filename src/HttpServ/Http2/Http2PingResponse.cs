using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class Http2PingResponse : Http2Response
    {
        public byte[] opaqueData { get; set; }

        public Http2PingResponse(byte[] opaqueData)
        {
            if (opaqueData == null)
                throw new ArgumentException("opaqueData is null");
            if (opaqueData.Length != 2)
                throw new ArgumentException("opaqueData.size is not 64");

            this.opaqueData = opaqueData;
        }
    }
}
