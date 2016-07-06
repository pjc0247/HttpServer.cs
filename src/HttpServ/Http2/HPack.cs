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

            InitializeStaticTable();
        }

        private void InitializeStaticTable()
        {
            SetAsMethod(2, "GET");
            SetAsMethod(3, "POST");
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
        public void SetAsHeader(int idx, string key, string value=null)
        {
            table[idx] = new HPackItem()
            {
                target = HPackTarget.Header,
                key = key,
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
            else if (ResponseCode.BadRequest == response.code)
                headers.Add(HPackCommandFactory.Indexed(PredefinedIndexes.Status400));
            else if (ResponseCode.NotFound == response.code)
                headers.Add(HPackCommandFactory.Indexed(PredefinedIndexes.Status404));
            else if (ResponseCode.InternalServerError == response.code)
                headers.Add(HPackCommandFactory.Indexed(PredefinedIndexes.Status500));
            else
                headers.Add(HPackCommandFactory.UpdateValue(PredefinedIndexes.Status1, ((int)response.code).ToString()));

            foreach(var header in response.headers)
            {
                // 이미 캐싱된 키
                if (tableByKey.ContainsKey(header.Key))
                {
                    var item = tableByKey[header.Key];

                    // 값도 캐싱됨
                    if (item.strValue == header.Value)
                        headers.Add(HPackCommandFactory.Indexed(item.index));
                    else
                        headers.Add(HPackCommandFactory.UseIndexedKey(item.index, header.Value));
                }
                else
                    headers.Add(HPackCommandFactory.NotIndexing(header.Key, header.Value));
            }

            return headers;
        }
    }
}
