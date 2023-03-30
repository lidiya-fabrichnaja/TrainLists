using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrainLists.Application.Models
{
    public class AppResponce
    {
        private readonly bool _success;

        private readonly IEnumerable<AppError> _errors
            = new AppError[0];

        private readonly string _message;

        public AppResponce()
        {
            _success = true;
        }

        public AppResponce(bool success){
            _success = success;
        }

        public AppResponce(IEnumerable<AppError> errors)
        {
            _success = false;
            _errors = errors;
        }

        public AppResponce(AppError error)
        {
            _success = false;
            _errors = new AppError[] { error };
        }

        public AppResponce(string message)
        {
            _success = true;
            _message = message;
        }

        public AppResponce(string message, bool success)
        {
            _success = success;
            _message = message;
        }

        
        public bool Success => _success;

        public IEnumerable<AppError> Errors => _errors;

        public string Message => _message;

    }
    public class AppResponce<TData> : AppResponce
    {
        private readonly TData _data;

        public AppResponce(TData data) : base()
        {
            _data = data;
        }

        public AppResponce(AppError error) : base(error)
        {

        }

        public AppResponce(string message) : base(message)
        {

        }

        public AppResponce(string message, bool seccusses) : base(message,seccusses)
        {

        }

        public AppResponce(IEnumerable<AppError> errors) : base(errors)
        {

        }


        public TData Data => _data;


    }

}
