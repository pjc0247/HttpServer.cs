﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http.Exceptions
{
    public class RequestTooBigException : CloseSessionException
    {
        public RequestTooBigException()
        {
            var response = new HttpResponse(ResponseCode.PayloadTooLarge);
            response.SetContent("PAYLOAD TOO LARGE");
            this.response = response;
        }
    }
}
