using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.WebSocket
{
    public class WebSocketMiddleware : IMiddleware
    {
        public void OnPostprocess(Session session, WebRequest request, WebResponse response)
        {
            var wsRequest = request as WebSocketRequest;
            var wsResponse = response as WebSocketResponse;

            if (wsRequest != null && wsResponse != null)
                OnPostprocess(session, wsRequest, wsResponse);
        }
        public WebResponse OnPreprocess(Session session, WebRequest request)
        {
            var wsRequest = request as WebSocketRequest;

            if (wsRequest != null)
                return OnPreprocess(session, wsRequest);
            else
                return null;
        }

        public virtual void OnPostprocess(Session session, WebSocketRequest request, WebResponse response)
        {
        }
        public virtual WebResponse OnPreprocess(Session session, WebSocketRequest request)
        {
            return null;
        }
    }
}
