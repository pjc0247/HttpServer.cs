using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http
{
    public class HttpKnownHeaders
    {
        public static readonly string Accept = "Accept";
        public static readonly string AcceptEncoding = "Accept-Encoding";
        public static readonly string AcceptLanguage = "Accept-Language";
        public static readonly string Authorization = "Authorization";
        public static readonly string Connection = "Connection";
        public static readonly string Cookie = "Cookie";
        public static readonly string Date = "Date";
        public static readonly string Expect = "Expect";
        public static readonly string Host = "Host";
        public static readonly string Origin = "Origin";
        public static readonly string Pragma = "Pragma";
        public static readonly string Upgrade = "Upgrade";
        public static readonly string UserAgent = "User-Agent";
        public static readonly string ContentLength = "Content-Length";
        public static readonly string ContentEncoding = "Content-Encoding";

        public static readonly string Status = "Status";
        public static readonly string Allow = "Allow";
        public static readonly string KeepAlive = "Keep-Alive";
    }
}
