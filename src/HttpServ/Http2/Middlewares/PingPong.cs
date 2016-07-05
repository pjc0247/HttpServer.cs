using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2.Middlewares
{
    public class PingPong : IMiddleware
    {
        public void OnPostprocess(Session session, WebRequest request, WebResponse response)
        {
        }
        public WebResponse OnPreprocess(Session session, WebRequest _request)
        {
            if (!(_request is Http2PingRequest)) return null;
            var request = (Http2PingRequest)_request;

            return new Http2PingResponse(request.opaqueData);
        }
    }
}
