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

        private string _requestUri;
        public string requestUri
        {
            get
            {
                return _requestUri;
            }
            set
            {
                var ps = value.Split(new char[] { '?', '&' });

                path = ps[0];
                foreach (var p in ps) {
                    var tokens = p.Split(new char[] { '=' }, 2);
                    if (tokens.Length == 2)
                        headers.Add(tokens[0], tokens[1]);
                }
                _requestUri = value;
            }
        }

        public string path { get; private set; }
        public Dictionary<string, string> parameters { get; set; }
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
