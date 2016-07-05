using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class Http2Parser
    {
        public static Http2Frame ParseHeader(ArraySegment<byte> data)
        {
            if (data.Count < 9) return null;

            var header = new Http2Frame();

            header.length = data.ElementAt(0) << 2 | data.ElementAt(1) << 1 | data.ElementAt(2);
            header.type = (Http2FrameType)data.ElementAt(3);
            header.streamId = BitConverter.ToInt32(data.ToArray(), 4);

            Console.WriteLine("HLENGTH : " + header.length.ToString());
            Console.WriteLine("TYPE : " + header.type.ToString()); 
            Console.WriteLine("STREAM ID : " + header.streamId);

            if (header.type == Http2FrameType.Headers)
            {
                Console.WriteLine("END HEADER : " + header.endHeaders.ToString());
                Console.WriteLine("PRIORITY : " + header.hasPriority.ToString());
                Console.WriteLine("PADDING : " + header.hasPadding.ToString());

                for(int i = 0; i < header.length; i++)
                {
                    Console.Write(data.ElementAt(9 + i).ToString() + " " );
                }
                Console.WriteLine();
            }

            return header;
        }

        public static Http2PingRequest ParsePing(Http2Frame frame, ArraySegment<byte> data)
        {
            if (frame.length > data.Count)
                return null;

            var req = new Http2PingRequest();

            req.opaqueData[0] = data.ElementAt(0);
            req.opaqueData[1] = data.ElementAt(1);

            Console.WriteLine("OPAQUE");
            Console.WriteLine(req.opaqueData[0]);
            Console.WriteLine(req.opaqueData[1]);

            return req;
        }
        public static Http2HeadersRequest ParseHeaders(Http2Frame frame, ArraySegment<byte> data)
        {
            if (frame.length > data.Count)
                return null;

            var req = new Http2HeadersRequest();

            for(int i = 0; i < frame.length; i++)
            {
                var ch = data.ElementAt(i);

                // 1 0 -> Indexed_HasNothing
                if ((ch & 0x80) != 0)
                    req.hpackCommands.Add(HPackCommandFactory.Indexed(ch ^ 0x80));
                // 0 1 -> Indexed_HasValue
                else if ((ch & 0x40) != 0)
                {
                    var length = data.ElementAt(i + 1);
                    var huff = (length & 0x80) != 0;

                    length &= 0x7F;

                    var payload = new byte[length];

                    Console.WriteLine("hUFF : " + huff.ToString());
                    Console.WriteLine("LEN : " + length.ToString());

                    Buffer.BlockCopy(data.Array, data.Offset + i + 2, payload, 0, length);
                    
                    if (huff)
                        payload  = Huffman.Convert.Decode(payload);
                    
                    Console.WriteLine(Encoding.UTF8.GetString(payload));

                    req.hpackCommands.Add(
                        HPackCommandFactory.UpdateValue(ch ^ 0x40, Encoding.UTF8.GetString(payload)));

                    i += 1 + length;
                }
            }

            return req;
        }
    }
}
