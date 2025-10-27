using EmployeeManagementSystem.Application.Interface;

using EmployeeManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Api.Filter
{
    public class ProjectTeamMemberFilter:IAsyncActionFilter
    {
        private readonly IAppDbContext dbContext;
        public ProjectTeamMemberFilter(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            

            // Get Employee Id from query string
            if (!context.HttpContext.Request.Query.TryGetValue("employeeId", out var empIdStr) ||
                !Guid.TryParse(empIdStr, out var employeeId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Fetch projects where the employee is a team member
            var projects = await dbContext.Projects
                .Include(p => p.TeamMember)
                .Where(p => p.TeamMember.Any(e => e.Id == employeeId))
                .ToListAsync();

            if (!projects.Any())
            {
                context.Result = new ForbidResult(); // Not part of any project
                return;
            }

            // Optionally, store employeeId/projects in HttpContext.Items for controller use
            context.HttpContext.Items["employeeId"] = employeeId;
            context.HttpContext.Items["projects"] = projects;

            await next();


        }
    }
}
