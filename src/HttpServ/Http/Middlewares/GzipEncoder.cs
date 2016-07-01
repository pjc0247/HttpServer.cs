using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace HttpServ.Http.Middlewares
{
    public class GzipEncoder : HttpMiddleware
    {
        private static readonly int GzipMinSize = 64;

        public override void OnPostprocess(Session session, HttpRequest request, HttpResponse response)
        {
            if (response.content == null) return;
            if (response.content.Length <= GzipMinSize) return;

            var accept = request.GetHeader(HttpKnownHeaders.AcceptEncoding);

            if (accept.Contains("gzip,"))
            {
                using (MemoryStream memory = new MemoryStream())
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(response.content, 0, response.content.Length);

                    var zippped = memory.ToArray();
                    response.content = zippped;
                    response.headers[HttpKnownHeaders.ContentEncoding] = "gzip";
                    response.headers[HttpKnownHeaders.ContentLength] = zippped.Length.ToString();
                }
            }
        }
    }
}
