using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class Http2FrameBuilder
    {
        private static readonly int Length8 = 2;
        private static readonly int Type = 3;
        private static readonly int Flags = 4;
        private static readonly int Payload = 9;

        public static byte[] Setting()
        {
            var data = new byte[9];

            data[Type] = 4;
            data[Flags] = 1;

            return data;
        }

        public static byte[] RstStream(Http2RstStreamResponse response)
        {
            var data = new byte[10];

            data[Length8] = 4;
            data[Type] = (int)Http2FrameType.RstStream;
            data[Flags] = 0;

            Buffer.BlockCopy(
                data, 10, BitConverter.GetBytes(response.errorCode), 0, 4);

            return data;
        }

        public static byte[] Pong(Http2PingResponse response)
        {
            var data = new byte[11];

            data[Length8] = 2;
            data[Type] = (int)Http2FrameType.Ping;
            data[Flags] = 0x1;
            data[Payload] = response.opaqueData[0];
            data[Payload+1] = response.opaqueData[1];

            return data;
        }
    }
}
