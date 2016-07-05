﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class Http2FrameBuilder
    {
        public static byte[] Setting()
        {
            var data = new byte[9];

            data[3] = 4;
            data[4] = 1;

            return data;
        }

        public static byte[] RstStream(Http2RstStreamResponse response)
        {
            var data = new byte[10];

            data[3] = (int)Http2FrameType.RstStream;
            data[4] = 0;

            Buffer.BlockCopy(
                data, 10, BitConverter.GetBytes(response.errorCode), 0, 4);

            return data;
        }
    }
}
