using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public class WebResponse
    {
        public byte[] content { get; set; }
        public bool isText { get; set; }

        public void SetContent(byte[] byteContent)
        {
            isText = false;
            content = byteContent;
        }
        public void SetContent(string stringContent)
        {
            isText = true;
            if (stringContent == null)
                content = new byte[] { };
            else
                content = Encoding.UTF8.GetBytes(stringContent);
        }
    }
}
