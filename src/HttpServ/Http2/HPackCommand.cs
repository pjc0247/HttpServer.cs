using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class HPackCommand
    {
        public HPackTarget target { get; set; }
        public HPackCommandType type { get; set; }

        public bool isIndexed { get; set; }
        public int index { get; set; }

        public string key { get; set; }

        public int intValue { get; set; }
        public string strValue { get; set; }
    }
}
