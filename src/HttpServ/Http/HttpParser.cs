using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http
{
    using Exceptions;

    public enum HttpParseResult
    {
        Processing,
        End,
        EndWithTrailing
    }
    public enum HttpParseState
    {
        Begin,

        EndMethod,
        EndRequestUri,

        BeginHeaderKey,
        EndHeaderKey,
        EndHeaderValue,
        RecvContent,

        End
    }

    public class HttpParser
    {
        private static readonly int MaxHttpMethodSize = 8;

        public byte[] buffer { get; private set; }

        private int offset { get; set; }
        private byte[] contentBuffer { get; set; }

        private HttpParseState state { get; set; }
        private string headerKey { get; set; }
        private int? contentLength { get; set; }

        public Action<string> OnHttpMethod { get; set; }
        public Action<string> OnRequestUri { get; set; }
        public Action<string> OnHttpVer { get; set; }
        public Action<string, string> OnHeader { get; set; }
        public Action<byte[]> OnContentChunk { get; set; }
        public Action<byte[]> OnContent { get; set; }
        public Action OnReset { get; set; }

        public bool allowContentWithoutLength = false;

        public HttpParser()
        {
            buffer = new byte[] { };
            contentBuffer = new byte[] { };

            OnReset += () =>
            {
                contentBuffer = new byte[] { };
            };
            OnHeader += (key, value) =>
            {
                if (key == HttpKnownHeaders.ContentLength)
                    contentLength = int.Parse(value);
            };
        }

        public void ForceSkipRequestLine()
        {
            state = HttpParseState.BeginHeaderKey;
        }
        public void ForceAllowContentWithoutLength()
        {
            allowContentWithoutLength = true;
        }

        public HttpParseResult Write(IEnumerable<byte> chunk)
        {
            if (chunk != null)
                buffer = buffer.Concat(chunk).ToArray();

            return Parse();
        }
        public HttpParseResult WriteEof()
        {
            if (state == HttpParseState.RecvContent)
            {
                OnContent?.Invoke(contentBuffer);
                return HttpParseResult.End;
            }
            else
                return HttpParseResult.Processing;
        }

        public HttpParseResult Parse()
        {
            if (state == HttpParseState.Begin)
                OnReset?.Invoke();

            while (true)
            {
                bool passed = false;

                switch (state)
                {
                    case HttpParseState.Begin:
                        passed = ParseHttpMethod();
                        break;
                    case HttpParseState.EndMethod:
                        passed = ParseRequestUri();
                        break;
                    case HttpParseState.EndRequestUri:
                        passed = ParseHttpVer();
                        break;

                    case HttpParseState.BeginHeaderKey:
                        passed = ParseHeaderKey();
                        break;
                    case HttpParseState.EndHeaderKey:
                        passed = ParseHeaderValue();
                        break;

                    case HttpParseState.RecvContent:
                        if (contentLength.HasValue &&
                            contentLength.Value < buffer.Length)
                        {
                            var slice = buffer.Take(contentLength.Value).ToArray();
                            buffer = buffer.Skip(contentLength.Value).ToArray();
                            OnContentChunk?.Invoke(slice);
                            contentBuffer = contentBuffer.Concat(slice).ToArray();
                        }
                        else
                        {
                            contentBuffer = contentBuffer.Concat(buffer).ToArray();
                            OnContentChunk?.Invoke(buffer);
                            buffer = new byte[] { };
                        }

                        if (contentLength.HasValue &&
                            contentLength == contentBuffer.Length)
                        {
                            OnContent?.Invoke(contentBuffer);
                            state = HttpParseState.End;
                        }

                        break;
                }

                if (passed == false || buffer.Length == 0)
                    break;
            }

            var parseEnd = state == HttpParseState.End;

            if (parseEnd) 
                state = HttpParseState.Begin;

            if (parseEnd && buffer.Length == 0)
                return HttpParseResult.End;
            else if (parseEnd)
                return HttpParseResult.EndWithTrailing;
            else
                return HttpParseResult.Processing;
        }

        private bool ParseHttpMethod()
        {
            if (buffer.Length >= 1 && char.IsLetter((char)buffer[0]) == false)
                throw new HttpParseException();
            if (buffer.Length < 4)
                return false;

            for (int i = 3; i < buffer.Length; i++)
            {
                if (i-3 >= MaxHttpMethodSize)
                    throw new HttpParseException("HTTP Method too long");

                if (buffer[i] == ' ')
                {
                    var method = buffer.Take(i).ToArray();
                    OnHttpMethod?.Invoke(Encoding.ASCII.GetString(method));
                    buffer = buffer.Skip(i + 1).ToArray();

                    state = HttpParseState.EndMethod;

                    return true;
                }
            }

            return false;
        }
        private bool ParseRequestUri()
        {
            if (buffer[0] != '/')
                ; //EXCEPTION

            for (int i = 1; i < buffer.Length; i++)
            {
                if (buffer[i] == ' ')
                {
                    var uri = buffer.Take(i).ToArray();
                    OnRequestUri?.Invoke(Encoding.ASCII.GetString(uri));
                    buffer = buffer.Skip(i + 1).ToArray();

                    state = HttpParseState.EndRequestUri;

                    return true;
                }
            }

            return false;
        }
        private bool ParseHttpVer()
        {
            for (int i = 1; i < buffer.Length - 1; i++)
            {
                if (buffer[i] == '\r' && buffer[i + 1] == '\n')
                {
                    var ver = buffer.Take(i).ToArray();;
                    OnHttpVer?.Invoke(Encoding.ASCII.GetString(ver));
                    buffer = buffer.Skip(i + 2).ToArray();

                    state = HttpParseState.BeginHeaderKey;

                    return true;
                }
            }

            return false;
        }

        private bool ParseHeaderKey()
        {
            if (buffer.Length < 2)
                return false;
            if (buffer[0] == '\r' && buffer[1] == '\n')
            {
                buffer = buffer.Skip(2).ToArray();

                if (contentLength.HasValue)
                    state = HttpParseState.RecvContent;
                else {
                    if (allowContentWithoutLength)
                        state = HttpParseState.RecvContent;
                    else
                        state = HttpParseState.End;
                }
                
                return true;
            }

            for (int i = 1; i < buffer.Length; i++)
            {
                if (buffer[i] == ':')
                {
                    var slice = buffer.Take(i).ToArray();
                    headerKey = Encoding.ASCII.GetString(slice).TrimEnd();
                    buffer = buffer.Skip(i + 1).ToArray();

                    state = HttpParseState.EndHeaderKey;

                    return true;
                }
            }

            return false;
        }
        private bool ParseHeaderValue()
        {
            for (int i = 1; i < buffer.Length - 1; i++)
            {
                if (buffer[i] == '\r' && buffer[i + 1] == '\n')
                {
                    var slice = buffer.Take(i).SkipWhile(x => (char)x == ' ').ToArray();
                    OnHeader?.Invoke(headerKey, Encoding.ASCII.GetString(slice));
                    buffer = buffer.Skip(i + 2).ToArray();

                    state = HttpParseState.BeginHeaderKey;

                    return true;
                }
            }

            return false;
        }
    }
}
