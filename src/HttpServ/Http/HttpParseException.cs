using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http
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
    }
}
