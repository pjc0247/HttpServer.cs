using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    internal class HStream
    {
        public HPack hpack { get; set; }

        public int priority { get; set; }
    }
}
