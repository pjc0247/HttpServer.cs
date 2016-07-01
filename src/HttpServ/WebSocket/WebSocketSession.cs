using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/*
 0               1               2               3              
 0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7 0 1 2 3 4 5 6 7
+-+-+-+-+-------+-+-------------+-------------------------------+
|F|R|R|R| opcode|M| Payload len |    Extended payload length    |
|I|S|S|S|  (4)  |A|     (7)     |             (16/64)           |
|N|V|V|V|       |S|             |   (if payload len==126/127)   |
| |1|2|3|       |K|             |                               |
+-+-+-+-+-------+-+-------------+ - - - - - - - - - - - - - - - +
 4               5               6               7              
+ - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - +
|     Extended payload length continued, if payload len == 127  |
+ - - - - - - - - - - - - - - - +-------------------------------+
 8               9               10              11             
+ - - - - - - - - - - - - - - - +-------------------------------+
|                               |Masking-key, if MASK set to 1  |
+-------------------------------+-------------------------------+
 12              13              14              15
+-------------------------------+-------------------------------+
| Masking-key (continued)       |          Payload Data         |
+-------------------------------- - - - - - - - - - - - - - - - +
:                     Payload Data continued ...                :
+ - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - +
|                     Payload Data continued ...                |
+---------------------------------------------------------------+
*/
namespace HttpServ.WebSocket
{
    using Exceptions;

    public class WebSocketSession : ISessionImpl
    {
        public Session session { get; set; }

        private WebSocketHeader header { get; set; }
        private byte[] contentBuffer { get; set; }
        private byte[] buffer { get; set; }

        public IEnumerable<WebRequest> OnReceiveData(ArraySegment<byte> data)
        {
            if (buffer == null)
                buffer = data.ToArray();
            else
                buffer = buffer.Concat(data).ToArray();

            // 이번 패킷에 대해서 아직 헤더 파싱 안됨
            if (header == null)
            {
                header = WebSocketParser.Parse(
                    new ArraySegment<byte>(buffer));
                contentBuffer = new byte[] { };

                var maxPayloadSize = session.server.config.maxWebSocketPayloadSize;

                // 들어온 데이터가 헤더 만들기 충분하지 않음
                if (header == null)
                    yield break;
                if (contentBuffer.Length + header.payloadLength >= maxPayloadSize)
                    throw new PayloadTooBigException();

                buffer = buffer.Skip(header.payloadOffset).ToArray();
            }

            // 버퍼가 헤더의 payload 길이보다 충분함
            //    -> 데이터 다 받은 경우
            if (buffer.Length >= header.payloadLength)
            {
                var payload = buffer.Take(header.payloadLength);
                buffer = buffer.Skip(header.payloadLength).ToArray();

                if (header.fin)
                {
                    var request = new WebSocketRequest();
                    IEnumerable<byte> content = null;

                    if (header.IsControlFrame())
                        request.content = Decode(payload).ToArray();
                    else
                    {
                        if (contentBuffer.Length > 0)
                            content = contentBuffer.Concat(payload);
                        else
                            content = payload;
                        request.content = Decode(content).ToArray();
                    }
                    request.opcode = (OpCode)header.opcode;

                    header = null;

                    yield return request;
                }
                else
                    contentBuffer = contentBuffer.Concat(payload).ToArray();

                if (buffer.Length > 0)
                {
                    foreach (var request in OnReceiveData(new ArraySegment<byte>()))
                        yield return request;
                }
            }
        }

        public byte[] OnWriteData(WebRequest _request, WebResponse _response)
        {
            var request = (WebSocketRequest)_request;
            var response = (WebSocketResponse)_response;

            var headerBytes = WebSocketParser.Build(
                new ArraySegment<byte>(response.content), (byte)response.opcode);

            return headerBytes.Concat(_response.content).ToArray();
        }

        public WebResponse OnErrorClose(Exception e)
        {
            return new WebSocketCloseResponse(
                StatusCode.ServerError, "Close connection due to an exception.");
        }

        private IEnumerable<byte> Decode(IEnumerable<byte> data)
        {
            if (header.mask == false)
            {
                foreach (var ch in data)
                    yield return ch;
            }
            else
            {
                int idx = 0;
                foreach (var ch in data)
                {
                    yield return (byte)(ch ^ header.GetMaskKeyAt(idx++));
                }
            }
        }
    }
}
