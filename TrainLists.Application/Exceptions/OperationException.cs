using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainLists.Application.Exceptions
{
    public class OperationException : Exception
    {
        public ErrorCode Code { get; }

        public HttpStatusCode StatusCode { get; }

        public OperationException(ErrorCode code, string message): base(message)
        {
            Code = code;
            StatusCode = HttpStatusCode.BadRequest;
        }

        public OperationException(ErrorCode code, string message, HttpStatusCode statusCode): base(message)
        {
            Code = code;
            StatusCode = statusCode;
        }

        
        
    }
}