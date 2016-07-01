using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http
{
    using Exceptions;

    public class HttpRequestBuilder
    {
        public HttpRequest request { get; internal set; }
        public HttpParser parser { get; private set; }

        private int maxContentSize { get; set; }
        private int contentSize { get; set; }

        public HttpRequestBuilder(int maxContentSize = int.MaxValue)
        {
            this.request = new HttpRequest();
            this.parser = new HttpParser();
            this.maxContentSize = maxContentSize;

            parser.OnReset += OnReset;
            parser.OnHttpMethod += OnMethod;
            parser.OnRequestUri += OnRequestUri;
            parser.OnHeader += OnHeader;
            parser.OnContent += OnContent;
            parser.OnContentChunk += OnContentChunk;
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
            contentSize = 0;
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

            if (key == HttpKnownHeaders.ContentLength)
            {
                int length;

                if (int.TryParse(value, out length))
                {
                    if (length >= maxContentSize)
                        throw new RequestTooBigException();
                }
                else
                    throw new HttpParseException();
            }
        }
        private void OnContent(byte[] receivedContent)
        {
            request.content = receivedContent;
        }
        private void OnContentChunk(byte[] receivedChunk)
        {
            contentSize += receivedChunk.Length;

            if (contentSize >= maxContentSize)
                throw new RequestTooBigException();
        }
    }
}
