using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class Http2RstStreamResponse : Http2Response
    {
        public int errorCode { get; set; }

        public Http2RstStreamResponse(int errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
