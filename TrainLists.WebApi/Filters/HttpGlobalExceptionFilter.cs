using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TrainLists.Application.Exceptions;
using TrainLists.Application.Models;

namespace TrainLists.WebApi.Filters
{
   public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(IWebHostEnvironment environment, ILogger<HttpGlobalExceptionFilter> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var errors = new Collection<AppError>();

            if (context.Exception is OperationException exp)
            {
                errors.Add(new AppError { Code = exp.Code.ToString(), Message = exp.Message });
                context.Result = new OkObjectResult(new AppResponce(errors));
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                context.ExceptionHandled = true;


                return;
            }

            

            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);


            if (_environment.IsDevelopment())
            {
                errors.Add(new AppError
                {
                    Code = "UnhandeledExeption",
                    Message = context.Exception.Message
                });
            }
            else
            {
                errors.Add(new AppError
                {
                    Code = "UnhandeledExeption",
                    Message = "Произошла непредвиденная ошибка. Повторите попытку позже"
                });
            }

            context.Result = new OkObjectResult(new AppResponce(errors));
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            context.ExceptionHandled = true;
        }
    }
}