using System;
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
    }
}
