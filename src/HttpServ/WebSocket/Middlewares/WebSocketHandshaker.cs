using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace HttpServ.WebSocket.Middlewares
{
    using Http;

    public class WebSocketHandshaker : HttpMiddleware
    {
        private static readonly string suffixGuid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

        private static readonly SHA1CryptoServiceProvider sha1Provider = new SHA1CryptoServiceProvider();

        public override HttpResponse OnPreprocess(Session session, HttpRequest request)
        {
            if (request.GetHeader(HttpKnownHeaders.Connection) != "Upgrade" ||
                request.GetHeader(HttpKnownHeaders.Upgrade) != "websocket")
                return null;

            var key = request.GetHeader("Sec-WebSocket-Key");
            var acceptKeyRaw = Encoding.UTF8.GetBytes(key + suffixGuid);
            
            var result = sha1Provider.ComputeHash(acceptKeyRaw);

            var acceptKey = Convert.ToBase64String(result);
            var response = new HttpResponse();

            response.code = ResponseCode.SwitchingProtocols;
            response.headers[HttpKnownHeaders.Connection] = "Upgrade";
            response.headers[HttpKnownHeaders.Upgrade] = "websocket";
            response.headers["Sec-WebSocket-Accept"] = acceptKey;

            // SUBPROTOCOL NOT SUPPORTED YET
            //response.headers["Sec-WebSocket-Protocol"] = "";

            session.UpgradeToWebSocket();

            return response;
        }

        public override void OnPostprocess(Session session, HttpRequest request, HttpResponse response)
        {
        }
    }
}
