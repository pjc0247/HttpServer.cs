using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http
{
    public static class HttpExt
    {
		public static void EnableGzipEncoding(this Server server)
        {
            server.AddMiddleware<Middlewares.GzipEncoder>();
        }
    }
}
