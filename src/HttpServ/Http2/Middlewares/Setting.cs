using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2.Middlewares
{
    public class Setting : IMiddleware
    {
        public void OnPostprocess(Session session, WebRequest request, WebResponse response)
        {
        }
        public WebResponse OnPreprocess(Session session, WebRequest request)
        {
            if (request is Http2SettingRequest)
                return new Http2SettingResponse();
            return null;
        }
    }
}
