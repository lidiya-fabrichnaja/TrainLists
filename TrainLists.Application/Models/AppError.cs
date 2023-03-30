using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainLists.Application.Models
{
    public class AppError
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public AppError(string message)
        {
            Message = message;
        }

        public AppError()
        {
            
        }

    }
}