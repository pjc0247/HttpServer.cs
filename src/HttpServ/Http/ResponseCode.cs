using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServ.Http
{
    public enum ResponseCode
    {
        Continue = 100,
        SwitchingProtocols = 101,
        Processing = 102,

        OK = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        ResetContent = 205,
        PartialContent = 206,

        MultipleChoices = 300,
        MovedPermanently = 301,
        Found = 302,

        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PayloadTooLarge = 413,
        UriTooLong = 414,

        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavaliable = 503,
        GatewayTimeout = 504,
    }
}
