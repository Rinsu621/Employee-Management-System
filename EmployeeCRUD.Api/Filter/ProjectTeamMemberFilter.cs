using EmployeeCRUD.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRUD.Api.Filter
{
    public class ProjectTeamMemberFilter:IAsyncActionFilter
    {
        private readonly AppDbContext dbContext;
        public ProjectTeamMemberFilter(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            if (!context.HttpContext.Request.Query.TryGetValue("employeeId", out var empIdStr) ||
                !Guid.TryParse(empIdStr, out var employeeId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var projects = await dbContext.Projects
                .Include(p => p.TeamMember)
                .Where(p => p.TeamMember.Any(e => e.Id == employeeId))
                .ToListAsync();

            if (!projects.Any())
            {
                context.Result = new ForbidResult(); 
                return;
            }
            context.HttpContext.Items["employeeId"] = employeeId;
            context.HttpContext.Items["projects"] = projects;

            await next();


        }
    }
}
