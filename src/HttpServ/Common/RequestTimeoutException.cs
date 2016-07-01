using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public class RequestTimeoutException : Exception
    {
        public RequestTimeoutException()
        {
        }
    }
}
