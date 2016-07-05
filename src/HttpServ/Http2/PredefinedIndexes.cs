using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http2
{
    public class PredefinedIndexes
    {
        public static readonly int Authority = 1;
        public static readonly int Method1 = 2;
        public static readonly int Method2 = 3;
        public static readonly int Path1 = 4;
        public static readonly int Path2 = 5;
        public static readonly int SchemeHttp = 6;
        public static readonly int SchemeHttps = 7;
        
        public static readonly int Status1 = 8;
        
        public static readonly int AcceptCharset = 15;
        public static readonly int AcceptEncoding = 16;
        public static readonly int AcceptLanguage = 17;
        public static readonly int AcceptRanges = 18;
        public static readonly int Accept = 19;
        
        public static readonly int AccessControlAllowOrigin = 20;
        
        public static readonly int Age = 21;
        public static readonly int Allow = 22;
        public static readonly int Authorization = 23;
        public static readonly int CacheControl = 24;
        
        public static readonly int ContentDisposition = 25;
        public static readonly int ContentEncoding = 26;
        public static readonly int ContentLanguage = 27;
        public static readonly int ContentLength = 28;
        public static readonly int ContentLocation = 29;
        public static readonly int ContentRange = 30;
        public static readonly int ContentType = 31;
        
        public static readonly int Cookie = 32;
        public static readonly int Date = 33;
        public static readonly int Etag = 34;
        public static readonly int Expect = 35;
    }
}
