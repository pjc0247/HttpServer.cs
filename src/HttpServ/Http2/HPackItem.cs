using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class HPackItem
    {
        public HPackTarget target { get; set; }

        public int index { get; set; }

        public string key { get; set; }
        public int intValue { get; set; }
        public string strValue { get; set; }
    }
}
