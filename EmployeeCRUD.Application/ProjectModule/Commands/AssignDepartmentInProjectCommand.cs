using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Application.ProjectModule.Dtos;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Data.keyless;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Commands
{
    public record AssignDepartmentInProjectCommand(Guid ProjectId, Guid DepartmentId): IRequest<ProjectDepartmentDto>;

    public class AssignDepartmentInProjectHandler : IRequestHandler<AssignDepartmentInProjectCommand, ProjectDepartmentDto>
    {
        private readonly IAppDbContext dbContext;

        public AssignDepartmentInProjectHandler(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ProjectDepartmentDto> Handle(AssignDepartmentInProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            var department = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == request.DepartmentId, cancellationToken);
            project.DepartmentId = request.DepartmentId;
            dbContext.Projects.Update(project);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new ProjectDepartmentDto
            {
                ProjectName = project.ProjectName,
                DepartmentName = department.DeptName
            };
        }

    }
  
}
