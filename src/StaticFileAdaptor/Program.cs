using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HttpServ.StaticFileAdaptor
{
    public class StaticFileAdaptor : IAdaptor
    {
        public void OpOpen(Session session)
        {
            Console.WriteLine("OnOpen");
        }
        public void OnClose(Session session)
        {
            Console.WriteLine("OnClose");
        }

        public WebResponse OnRequest(Session session, WebRequest req)
        {
            if (req is Http.HttpRequest)
                return OnHttpRequest((Http.HttpRequest)req);
            else if (req is WebSocket.WebSocketRequest)
                throw new NotImplementedException("This adaptor does not support WebSocket protocol.");

            return null;
        }

        private Http.HttpResponse OnHttpRequest(Http.HttpRequest request)
        {
            Console.WriteLine("StaticFileAdaptor::OnHttpRequest " + request.requestUri);

            var response = new Http.HttpResponse();
            var path = request.requestUri.Substring(1).Replace("/", "\\");
            
            if (path.Contains("..\\"))
            {
                response.code = Http.ResponseCode.Forbidden;
                response.reasonPhrase = "Forbidden";
            }
            else if (File.Exists(path) == false)
            {
                response.code = Http.ResponseCode.NotFound;
                response.reasonPhrase = "NotFound";
            }
            else
            {
                response.code = Http.ResponseCode.OK;
                response.reasonPhrase = "OK";

                response.headers["Content-Type"] = "application/octet-stream";

                using (var fp = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (fp.Length > int.MaxValue)
                        throw new NotImplementedException("size over INT_MAX");

                    response.content = new byte[fp.Length];

                    fp.Read(response.content, 0, 
                        Convert.ToInt32(fp.Length));
                }
            }

            return response;
        }
    }
}
