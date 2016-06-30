using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ
{
    public partial class Config
    {
        public int maxRequestSize { get; set; } = 16;

        public bool isDebugMode { get; set; } = false;

        /// <summary>
        /// 500 에러가 발생했을 때의 커스텀 렌더 콜백
        /// </summary>
        public Func<Exception, string> onInternalServerError { get; set; }
    }
}
