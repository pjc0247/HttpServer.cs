using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2.Middlewares
{
    using Http;

    public class PrefaceHandshake : IMiddleware
    {
        private static readonly string PrefaceMethod = "PRI";
        private static readonly string PrefaceContent = "SM\r\n\r\n";

        public void OnPostprocess(Session session, WebRequest request, WebResponse response)
        {
        }
        public WebResponse OnPreprocess(Session session, WebRequest _request)
        {
            if (!(_request is HttpRequest)) return null;

            var request = (HttpRequest)_request;

            if (request.method != PrefaceMethod) return null;
            if (request.content.SequenceEqual(Encoding.UTF8.GetBytes(PrefaceContent)) == false) return null;

            session.UpgradeToHttp2();

            return new Http2SettingResponse();
        }
    }
}
 