using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Simple
{
    public class SimpleAdaptor : IAdaptor
    {
        public void OpOpen(Session session)
        {
            Console.WriteLine("OnOpen");
        }
        public void OnClose(Session session)
        {
            Console.WriteLine("OnClose");
        }

        public bool OnUpgradeRequest(Session session, Type upgradeTo)
        {
            return true;
        }

        public WebResponse OnRequest(Session session, WebRequest req)
        {
            if (req is Http.HttpRequest)
                return OnHttpRequest((Http.HttpRequest)req);
            else if (req is WebSocket.WebSocketRequest)
                return OnWebSocketRequest((WebSocket.WebSocketRequest)req);

            return null;
        }

        /// <summary>
        /// HTTP/1.1 요청이 들어오면 이곳에서 처리합니다.
        /// </summary>
        /// <param name="request">HTTP 요청 데이터</param>
        /// <returns>HTTP 리스폰스</returns>
        private Http.HttpResponse OnHttpRequest(Http.HttpRequest request)
        {
            Console.WriteLine("SimpleAdaptor::OnHttpRequest " + request.requestUri);

            var response = new Http.HttpResponse();

            response.code = Http.ResponseCode.OK;
            response.reasonPhrase = "OK";
            response.SetContent("Hello World! " + request.requestUri);

            throw new CloseSessionException(response);

            //return response;
        }

        /// <summary>
        /// WEBSOCKET 요청이 들어오면 이곳에서 처리합니다.
        /// </summary>
        /// <param name="request">WEBSOCKET 패킷</param>
        /// <returns>WEBSOCKET 리스폰스</returns>
        private WebSocket.WebSocketResponse OnWebSocketRequest(WebSocket.WebSocketRequest request)
        {
            Console.WriteLine("SimpleAdaptor::OnWebSocketRequest - " + request.opcode.ToString());
            Console.WriteLine(request.GetContentAsString());

            var response = new WebSocket.WebSocketResponse();

            response.SetContent("Hello World! ");
            response.opcode = WebSocket.OpCode.Close;

            throw new CloseSessionException(response);
            //return response;
        }
    }
}
