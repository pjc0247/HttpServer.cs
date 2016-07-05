using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class Http2Frame
    {
        public int length { get; set; }
        public Http2FrameType type { get; set; }
        public ushort flags { get; set; }
        public int streamId { get; set; }

        public bool endStream
        {
            get
            {
                return (flags & 0x1) != 0;
            }
        }
        public bool hasPadding
        {
            get
            {
                return (flags & 0x8) != 0;
            }
        }
        public bool hasPriority
        {
            get
            {
                return (flags & 0x20) != 0;
            }
        }
        public bool endHeaders
        {
            get
            {
                return (flags & 0x4) != 0;
            }
        }
    }
}
