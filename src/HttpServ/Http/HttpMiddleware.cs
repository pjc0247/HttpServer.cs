using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http
{
    public class HttpMiddleware : IMiddleware
    {
        public void OnPostprocess(Session session, WebRequest request, WebResponse response)
        {
            var httpRequest = request as HttpRequest;
            var httpResponse = response as HttpResponse;

            if (httpRequest != null && httpResponse != null)
                OnPostprocess(session, httpRequest, httpResponse);
        }
        public WebResponse OnPreprocess(Session session, WebRequest request)
        {
            var httpRequest = request as HttpRequest;

            if (httpRequest != null)
                return OnPreprocess(session, httpRequest);
            else
                return null;
        }

        public virtual void OnPostprocess(Session session, HttpRequest request, HttpResponse response)
        {
        }
        public virtual HttpResponse OnPreprocess(Session session, HttpRequest request)
        {
            return null;
        }
    }
}
