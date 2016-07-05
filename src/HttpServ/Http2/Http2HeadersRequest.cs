using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class Http2HeadersRequest : WebRequest
    {
        public bool hasPadding { get; set; }
        public bool hasPriority { get; set; }

        public int streamId { get; set; }

        public List<HPackCommand> hpackCommands { get; set; }

        public Http2HeadersRequest()
        {
            hpackCommands = new List<HPackCommand>();
        }
    }
}
