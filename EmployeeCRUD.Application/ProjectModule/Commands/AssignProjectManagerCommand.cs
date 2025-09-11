using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Application.ProjectModule.Dtos;
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
    public record AssignProjectManagerCommand(Guid ProjectId, Guid EmployeeId) : IRequest<ProjectManagerAssignmentDto>;

    public class AssignProjectManagerHandler : IRequestHandler<AssignProjectManagerCommand, ProjectManagerAssignmentDto>
    {
        private readonly IAppDbContext dbContext;

        public AssignProjectManagerHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<ProjectManagerAssignmentDto> Handle(AssignProjectManagerCommand request, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            var employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);
            project.ProjectManagerId = request.EmployeeId;
            project.ProjectManager = employee;

            dbContext.Projects.Update(project);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new ProjectManagerAssignmentDto
            {

                ProjectName = project.ProjectName,
                Status = project.Status,
                ProjectManagerName = employee.EmpName  // Assuming the employee has a `Name` property
            };
        }

    }
 }
