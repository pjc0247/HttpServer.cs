using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public interface IMiddleware
    {
        /// <summary>
        /// Adaptor로 요청이 전달되기 전 처리를 수행합니다.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        WebResponse OnPreprocess(Session session, WebRequest request);

        /// <summary>
        /// Adaptor로부터 응답이 도착했을 때 후처리를 수행합니다.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="request">요청 (nullable)</param>
        /// <param name="response">Adaptor가 반환한 응답</param>
        void OnPostprocess(Session session, WebRequest request, WebResponse response);
    }
}
