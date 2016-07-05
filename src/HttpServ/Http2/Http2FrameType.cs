using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public enum Http2FrameType
    {
        Data = 0,
        Headers = 1,
        Priority = 2,
        RstStream = 3,
        Setting = 4,

    }
}
