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

        public HttpSession(Server server)
        {
            builder = new HttpRequestBuilder(server.config.maxRequestSize);
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

            if (response.content != null &&
                response.headers.ContainsKey(HttpKnownHeaders.ContentLength) == false)
                response.headers[HttpKnownHeaders.ContentLength] = response.content.Length.ToString();

            if (request != null && request.GetHeader(HttpKnownHeaders.Connection) == "keep-alive")
            {
                response.headers[HttpKnownHeaders.KeepAlive] = $"timeout=5, max={keepAliveCount}";
                keepAliveCount--;

                if (keepAliveCount > 0)
                    session.AcceptNextRequest();
            }

            return BuildHttpResponse(response);
        }

        public WebResponse OnErrorClose(Exception e)
        {
            var response = new HttpResponse(ResponseCode.InternalServerError);

            try
            {
                // 콜백 지정하지 않았을 경우 자동으로 catch에서처리됨
                response.SetContent(
                    session.server.config.onInternalServerError(e));
            }
            catch(Exception)
            {
                if (session.server.config.isDebugMode)
                    response.SetContent(e.ToString());
                else
                    response.SetContent("<h1>Internal Server Error</h1><hr>");
            }

            return response;
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
