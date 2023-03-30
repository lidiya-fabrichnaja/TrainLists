using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainLists.Application.Exceptions
{
    public enum ErrorCode : byte
    {
        AccessDenied,
        NotFound,
        InternalError

    }
}