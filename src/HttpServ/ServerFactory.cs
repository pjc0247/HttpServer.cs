using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public class ServerFactory
    {
        public static Server Create<T>()
            where T : IAdaptor, new()
        {
            return new Server(new T());
        }
    }
}
