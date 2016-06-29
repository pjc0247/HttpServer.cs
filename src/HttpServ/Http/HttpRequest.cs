using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http
{
    public class HttpRequest : WebRequest
    {
        public string method { get; set; }
        public string requestUri { get; set; }
        public Dictionary<string, string> headers { get; set; }

        public HttpRequest()
        {
            headers = new Dictionary<string, string>();
        }

        public string GetHeader(string key, string deafultValue = "")
        {
            if (headers.ContainsKey(key))
                return headers[key];
            return deafultValue;
        }
    }
}
