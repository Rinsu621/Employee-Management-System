using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Authorization
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public string[] AllowedPermissions { get; }
        //what a permission requirement is and a custom claim type that we’ll use in JWT or claims transformation.
        public PermissionAuthorizationRequirement(params string[] allowedPermissions)
        {
            AllowedPermissions = allowedPermissions;
        }
    }
}
