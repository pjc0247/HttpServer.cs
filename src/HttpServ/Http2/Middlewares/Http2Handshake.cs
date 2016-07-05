using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2.Middlewares
{
    using Http;

    public class Http2Handshake : HttpMiddleware
    {
        public override HttpResponse OnPreprocess(Session session, HttpRequest request)
        {
            if (request.GetHeader(HttpKnownHeaders.Upgrade) != "h2c")
                return null;

            var response = new HttpResponse(ResponseCode.SwitchingProtocols);

            response.headers[HttpKnownHeaders.Upgrade] = "h2c";
            response.headers[HttpKnownHeaders.Connection] = "upgrade";

            return response;
        }
    }
}
