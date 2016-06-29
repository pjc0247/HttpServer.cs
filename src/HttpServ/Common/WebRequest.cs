using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public class WebRequest
    {
        public byte[] content { get; set; }

        public string GetContentAsString()
        {
            return Encoding.UTF8.GetString(content);
        }
    }
}
