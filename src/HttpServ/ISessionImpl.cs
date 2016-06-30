using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public interface ISessionImpl
    {
        Session session { get; set; }

        /// <summary>
        /// 이 메소드를 구현하여 byte 데이터를 WebRequest로 변환한다.
        /// </summary>
        /// <param name="data">수신된 바이너리 데이터</param>
        /// <returns>(열거형) 바이너리로부터 빌드된 요청 목록</returns>
        IEnumerable<WebRequest> OnReceiveData(ArraySegment<byte> data);

        /// <summary>
        /// 이 메소드를 구현하여 WebResponse를 바이너리 데이터로 변환한다.
        /// </summary>
        /// <param name="request">WebResponse를 생성하는데 사용된 request (nullable)</param>
        /// <param name="response">바이너리로 변환해야 하는 WebResponse</param>
        /// <returns>변환된 바이너리 배열</returns>
        byte[] OnWriteData(WebRequest request, WebResponse response);

        /// <summary>
        /// 이 메소드를 구현하여 에러 종료시에 어떤 응답을 보낼지 알려준다.
        /// </summary>
        /// <param name="e">발생한 익셉션</param>
        /// <returns>마지막 패킷</returns>
        WebResponse OnErrorClose(Exception e);
    }
}
