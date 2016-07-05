using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class Http2Session : ISessionImpl
    {
        public Session session { get; set; }

        private Dictionary<int, HStream> streams { get; set; }

        public IEnumerable<WebRequest> OnReceiveData(ArraySegment<byte> data)
        {
            var header = Http2Parser.ParseHeader(data);

            if (header == null)
                yield break;

            HStream stream = null;

            if (header.streamId != 0 &&
                streams.ContainsKey(header.streamId))
                stream = streams[header.streamId];

            if (header.type == Http2FrameType.Setting)
                yield return new Http2SettingRequest();
            else if (header.type == Http2FrameType.Headers)
            {
                var headers = Http2Parser.ParseHeaders(
                    header, new ArraySegment<byte>(data.Array, data.Offset + 9, data.Count - 9));

                if (headers == null)
                    yield break;

                if (streams.ContainsKey(header.streamId) == false)
                    stream = streams[header.streamId] = new HStream();

            }
            else if (header.type == Http2FrameType.Ping)
            {
                var ping = Http2Parser.ParsePing(
                    header, new ArraySegment<byte>(data.Array, data.Offset + 9, data.Count - 9));

                if (ping == null)
                    yield break;

                yield return ping;
            }
        }

        public byte[] OnWriteData(WebRequest request, WebResponse response)
        {
            if (response is Http2SettingResponse)
                return Http2FrameBuilder.Setting();
            else if (response is Http2PingResponse)
                return Http2FrameBuilder.Pong((Http2PingResponse)response);

            return new byte[] { };
        }

        public WebResponse OnErrorClose(Exception e)
        {
            return new WebResponse();
        }
    }
}
