using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public enum HPackTarget
    {
        Auth,
        Method,
        Path,
        Scheme,
        Status,
        Header
    }
}
