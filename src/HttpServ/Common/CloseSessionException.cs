using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    /// <summary>
    /// </summary>
    /// <example>
    /// if (errorState)
    ///     throw new CloseSessionException();
    /// </example>
    public class CloseSessionException : Exception
    {
        public WebResponse response { get; set; }

        /// <summary>
        /// 세션을 강제로 닫는다.
        /// </summary>
        public CloseSessionException()
        {
        }

        /// <summary>
        /// 마지막 패킷을 보내고 세션을 닫는다.
        /// .
        /// 웹소켓일 경우 Opcode.Close 패킷을 넣어서 정상적인 close
        /// 핸드쉐이킹을 수행하고 연결을 종료할 수 있다.
        /// </summary>
        /// <param name="response">마지막으로 보낼 응답</param>
        public CloseSessionException(WebResponse response)
        {
            this.response = response;
        }
    }
}
