using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public interface IAdaptor
    {
        void OpOpen(Session session);
        void OnClose(Session session);

        WebResponse OnRequest(Session session, WebRequest req);
    }
}
