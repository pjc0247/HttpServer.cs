using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public class ServerFactory
    {
        public static Server CreateHttp<T>()
            where T : IAdaptor, new()
        {
            return new Server(new T(), null);
        }
        public static Server CreateHttps<T>(
            X509Certificate2 cert)
            where T : IAdaptor, new()
        {
            return new Server(new T(), cert);
        }
    }
}
