using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    internal class HPackCommandFactory
    {
        /// <summary>
        /// 이미 테이블에 있고,
        /// 키와 밸류 모두 없는 커맨드 생성
        /// </summary>
        /// <param name="index">이미 존재하는 인덱스</param>
        /// <returns></returns>
        public static HPackCommand Indexed(int index)
        {
            return new HPackCommand()
            {
                type = HPackCommandType.Indexed_HasNothing,
                index = index
            };
        }

        /// <summary>
        /// 키는 그대로 사용하고,
        /// 밸류만 업데이트하는 커맨드 생성
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HPackCommand UpdateValue(int index, string value)
        {
            return new HPackCommand()
            {
                type = HPackCommandType.Indexed_HasValue,
                index = index,
                strValue = value
            };
        }
        
        public static HPackCommand NotIndexing(string key, string value)
        {
            return new HPackCommand()
            {
                type = HPackCommandType.NoIndexing_HasKeyValue,
                index = 0,
                key = key,
                strValue = value
            };
        }
        public static HPackCommand UseIndexedKey(int index, string value)
        {
            return new HPackCommand()
            {
                type = HPackCommandType.NoIndexing_HasValue,
                index = index,
                strValue = value
            };
        }
    }
}
