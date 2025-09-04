using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Exceptions
{
    public class CustomValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }
        public CustomValidationException(IDictionary<string, string[]> errors)
           : base("Validation Failed")
        {
            Errors = errors;
        }

    }
}
