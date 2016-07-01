using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public enum SessionState
    {
        Ready,
        Opened,
        /// <summary>
        /// 소켓이 닫히지는 않았지만,
        /// 종료 핸드쉐이킹이 진행중인 상태
        /// </summary>
        Closing,
        /// <summary>
        /// 종료 핸드쉐이킹 완료 또는 에러에 의해
        /// 소켓이 완전히 닫힌 상태
        /// </summary>
        Closed
    }
}
