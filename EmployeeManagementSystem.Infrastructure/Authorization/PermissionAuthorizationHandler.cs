using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Authorization
{
    public class PermissionAuthorizationHandler:AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
           foreach(var permission in requirement.AllowedPermissions)
            {
                bool hasPermission = context.User.Claims.Any(c => c.Type == CustomClaimTypes.Permission && c.Value == permission);
                if(hasPermission)
                {
                    context.Succeed(requirement);
                    break;
                }
            }
           return Task.CompletedTask;
        }
    }
}
