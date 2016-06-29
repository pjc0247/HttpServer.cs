using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    /// <summary>
    /// 프로토콜이 업그레이드 가능함을 나타낸다.
    /// </summary>
    public interface IProtocolUpgradable
    {
        /// <summary>
        /// 이 메소드를 구현하여 현재 프로토콜에서 파싱 후 남은 데이터를 반환한다.
        /// 반환된 값은 스위칭된 프로토콜 구현체 파서에게 전달된다.
        /// </summary>
        /// <returns>파싱 후 남은 데이터</returns>
        byte[] GetTrailingData();
    }
}
