using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.Exceptions
{
    public class AuthorizationException: Exception
    {
        public AuthorizationException(string message="You are not allowed to perform this action"):base(message)
        { }
    }
}
