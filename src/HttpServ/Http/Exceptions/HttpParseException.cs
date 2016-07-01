using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http.Exceptions
{
    public class HttpParseException : CloseSessionException
    {
        public HttpParseException()
        {
            var response = new HttpResponse();
            response.SetContent("BAD REQUEST");
            response.code = ResponseCode.BadRequest;

            this.response = response;
        }
        public HttpParseException(string reason)
        {
            var response = new HttpResponse();
            response.SetContent(reason);
            response.code = ResponseCode.BadRequest;

            this.response = response;
        }
    }
}
