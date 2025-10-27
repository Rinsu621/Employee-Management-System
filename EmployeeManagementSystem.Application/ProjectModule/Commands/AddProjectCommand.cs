using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Infrastructure.Data;
using EmployeeManagementSystem.Infrastructure.Data.keyless;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.ProjectModule.Commands
{
    public record AddProjectCommand(string ProjectName, string Description, DateTime StartDate, DateTime? EndDate, decimal Budget, string Status, string? ClientName): IRequest<ProjectCreateKeyless>;

    public class  AddProjectHandler:IRequestHandler<AddProjectCommand, ProjectCreateKeyless>
    {
        private readonly IAppDbContext dbContext;
        public  AddProjectHandler(IAppDbContext _dbContext)
        {
           dbContext = _dbContext;
        }

        public async Task<ProjectCreateKeyless > Handle(AddProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project
            {
                ProjectName = request.ProjectName,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Budget = request.Budget,
                Status = request.Status,
                ClientName = request.ClientName
            };
            dbContext.Projects.Add(project);
            await dbContext.SaveChangesAsync(cancellationToken);
            var result = new ProjectCreateKeyless
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Budget = project.Budget,
                Status = project.Status,
                ClientName = project.ClientName
            };
            return result;


        }

    }

}
