using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http
{
    public class HttpSession : ISessionImpl, IProtocolUpgradable
    {
        public Session session { get; set; }

        private HttpRequestBuilder builder { get; set; }
        private int keepAliveCount = 100;

        public HttpSession()
        {
            builder = new HttpRequestBuilder();
        }

        public IEnumerable<WebRequest> OnReceiveData(ArraySegment<byte> data)
        {
            var server = session.server;

            if (data.Count <= 0)
                yield return builder.WriteEof();
            else
            {
                foreach(var request in builder.Write(data))
                {
                    yield return request;
                }
            }
        }

        public byte[] OnWriteData(WebRequest _request, WebResponse _response)
        {
            var request = (HttpRequest)_request;
            var response = (HttpResponse)_response;

            if (response.content != null && response.headers.ContainsKey("Content-Length") == false)
                response.headers["Content-Length"] = response.content.Length.ToString();

            if (request != null && request.GetHeader("Connection") == "keep-alive")
            {
                response.headers["Keep-Alive"] = $"timeout=5, max={keepAliveCount}";
                keepAliveCount--;

                if (keepAliveCount > 0)
                    session.AcceptNextRequest();
            }

            return BuildHttpResponse(response);
        }

        private byte[] BuildHttpResponse(HttpResponse response)
        {
            var httpHeader = "";

            httpHeader = $"HTTP/1.1 {(int)response.code} {response.reasonPhrase}\r\n";
            foreach(var header in response.headers)
            {
                httpHeader += $"{header.Key}: {header.Value}\r\n";
            }
            httpHeader += "\r\n";

            var byteHeader = Encoding.UTF8.GetBytes(httpHeader);

            if (response.content != null)
                byteHeader = byteHeader.Concat(response.content).ToArray();

            return byteHeader;    
        }

        public byte[] GetTrailingData()
        {
            return builder.parser.buffer;
        }
    }
}
