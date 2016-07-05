using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public enum HPackCommandType
    {
        // 0 0 0 0 / 0
        NoIndexing_HasKeyValue,
        NoIndexing_HasValue,
        // 0 0 0 1 / 0
        NeverIndexing_HasKeyValue,
        NeverIndexing_HasValue,

        // 1 / INDEX
        Indexed_HasNothing,
        // 0 1 / 0
        Indexed_HasKeyValue,
        // 0 1 / INDEX
        Indexed_HasValue
    }
}
