using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class Http2Request : WebRequest
    {
        public int streamId { get; set; }

    }
}
