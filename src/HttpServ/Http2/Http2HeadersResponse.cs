using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class Http2HeadersResponse : Http2Response
    {
        public List<HPackCommand> hpackCommands { get; set; }

        public Http2HeadersResponse()
        {
            hpackCommands = new List<HPackCommand>();
        }

        // shortcut
        public void Add(HPackCommand add)
        {
            hpackCommands.Add(add);
        }
    }
}
