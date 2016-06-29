using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http
{
    public class HttpResponse : WebResponse
    {
        public ResponseCode code { get; set; }
        public string reasonPhrase { get; set; }
        public Dictionary<string, string> headers { get; set; }

        public HttpResponse(ResponseCode code = ResponseCode.InternalServerError)
        {
            this.code = code;
            this.headers = new Dictionary<string, string>();
        }
    }
}
