using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public interface IMiddleware
    {
        WebResponse OnPreprocess(Session session, WebRequest request);
        void OnPostprocess(Session session, WebRequest request, WebResponse response);
    }
}
