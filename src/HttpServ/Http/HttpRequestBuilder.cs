using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http
{
    public class HttpRequestBuilder
    {
        public HttpRequest request { get; internal set; }
        public HttpParser parser { get; private set; }

        public HttpRequestBuilder()
        {
            request = new HttpRequest();
            parser = new HttpParser();

            parser.OnReset += OnReset;
            parser.OnHttpMethod += OnMethod;
            parser.OnRequestUri += OnRequestUri;
            parser.OnHeader += OnHeader;
            parser.OnContent += OnContent;
        }

        public IEnumerable<HttpRequest> Write(IEnumerable<byte> chunk)
        {
            while (parser.Write(chunk) != HttpParseResult.Processing)
            {
                chunk = new byte[] { };
                yield return request;
            }
        }
        public HttpRequest WriteEof()
        {
            if (parser.WriteEof() == HttpParseResult.Processing)
                return null;
            return request;
        }

        private void OnReset()
        {
            request = new HttpRequest();
        }
        private void OnMethod(string receivedMethod)
        {
            request.method = receivedMethod;
        }
        private void OnRequestUri(string receivedUri)
        {
            request.requestUri = receivedUri;
        }
        private void OnHeader(string key, string value)
        {
            request.headers[key] = value;
        }
        private void OnContent(byte[] receivedContent)
        {
            request.content = receivedContent;
        }
    }
}
