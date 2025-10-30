using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Authorization
{
    public static class PermissionExtensions
    {
        public static void RequirePermission(
            this AuthorizationPolicyBuilder builder,
            params string[] allowedPermissions)
        {
            builder.AddRequirements(new PermissionAuthorizationRequirement(allowedPermissions));
        }
    }
}
