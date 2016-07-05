using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    using Http;

    internal class HPack
    {
        private Dictionary<int, HPackItem> table { get; set; }

        private Dictionary<string, HPackItem> tableByKey { get; set; }

        public HPack()
        {
            table = new Dictionary<int, HPackItem>();

            InitializePredefinedData();
        }

        private void InitializePredefinedData()
        {
        }

        public HPackItem Get(int idx)
        {
            return table[idx];
        }
        public void SetAsMethod(int idx, string value)
        {
            table[idx] = new HPackItem()
            {
                target = HPackTarget.Method,
                strValue = value
            };
        }

        public HttpRequest Build()
        {
            return null;
        }
        public Http2HeadersResponse BuildResponse(HttpResponse response)
        {
            var headers = new Http2HeadersResponse();

            if (ResponseCode.OK == response.code)
                headers.Add(HPackCommandFactory.Indexed(PredefinedIndexes.Status1));
            else
                headers.Add(HPackCommandFactory.UpdateValue(PredefinedIndexes.Status1, (int)response.code));

            return headers;
        }
    }
}
