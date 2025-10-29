using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.Common.Authorization
{
    public class DynamicSalaryRequirement:IAuthorizationRequirement
    {
        public string Action { get; }

        public DynamicSalaryRequirement(string action)
        {
            Action=action;
        }
    }
}
