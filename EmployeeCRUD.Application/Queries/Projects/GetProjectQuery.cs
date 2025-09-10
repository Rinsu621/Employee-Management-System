using EmployeeCRUD.Application.Dtos.Projects;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Data.keyless;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Queries.Projects
{
    //public record GetProjectQuery : IRequest<IEnumerable<ProjectDto>>;

    //public class GetProjectHandler : IRequestHandler<GetProjectQuery, IEnumerable<ProjectDto>>
    //{
    //    private readonly AppDbContext dbContext;

    //    public GetProjectHandler(AppDbContext _dbContext)
    //    {
    //        dbContext = _dbContext;
    //    }

    //    public async Task<IEnumerable<ProjectDto>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    //    {
    //        // Fetch projects without eager loading TeamMember using Include
    //        var projects = await dbContext.Project
    //            .Include(p => p.Department)  // Include Department if needed
    //            .Include(p => p.ProjectManager)
    //            .Include(p => p.TeamMember)// Include ProjectManager if needed
    //            .ToListAsync(cancellationToken);

    //        // Map the projects to ProjectCreateKeyless
    //        return projects.Select(p => new ProjectDto
    //        {
    //            Id = p.Id,
    //            ProjectName = p.ProjectName,
    //            Description = p.Description,
    //            StartDate = p.StartDate,
    //            EndDate = p.EndDate,
    //            Budget = p.Budget,
    //            Status = p.Status,
    //            ClientName = p.ClientName,
    //            DepartmentName = p.Department?.DeptName,
    //            ProjectManagerName = p.ProjectManager?.EmpName,
    //            TeamMember = p.TeamMember.Select(e => e.EmpName).ToList()
    //        });
    //    }
    //}

    public record GetProjectQuery(Guid? EmployeeId = null) : IRequest<IEnumerable<ProjectDto>>;

    public class GetProjectHandler : IRequestHandler<GetProjectQuery, IEnumerable<ProjectDto>>
    {
        private readonly AppDbContext dbContext;

        public GetProjectHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<IEnumerable<ProjectDto>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext.Project
                .Include(p => p.Department)
                .Include(p => p.ProjectManager)
                .Include(p => p.TeamMember)
                .AsQueryable();

            if (request.EmployeeId.HasValue)
                query = query.Where(p => p.TeamMember.Any(e => e.Id == request.EmployeeId.Value));

            var projects = await query.ToListAsync(cancellationToken);

            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Budget = p.Budget,
                Status = p.Status,
                ClientName = p.ClientName,
                DepartmentName = p.Department?.DeptName,
                ProjectManagerName = p.ProjectManager?.EmpName,
                TeamMember = p.TeamMember.Select(e => e.EmpName).ToList()
            });
        }
    }

}
