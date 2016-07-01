using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public interface IAdaptor
    {
        /// <summary>
        /// 이 메소드를 상속받아 새로운 연결에 대한 처리를 수행합니다.
        /// </summary>
        /// <param name="session"></param>
        void OpOpen(Session session);

        /// <summary>
        /// 이 메소드를 상속받아 연결이 종료될때의 처리를 수행합니다.
        /// </summary>
        /// <param name="session"></param>
        void OnClose(Session session);

        /// <summary>
        /// 이 메소드를 상속받아 요청에 대한 응답을 처리합니다.
        /// </summary>
        /// <param name="session">요청을 보낸 세션</param>
        /// <param name="req">요청</param>
        /// <returns>응답</returns>
        WebResponse OnRequest(Session session, WebRequest req);
    }
}
