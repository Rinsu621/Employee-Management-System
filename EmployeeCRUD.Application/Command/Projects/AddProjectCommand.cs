using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Data.keyless;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Projects
{
    public record AddProjectCommand(string ProjectName, string Description, DateTime StartDate, DateTime? EndDate, decimal Budget, string Status, string? ClientName): IRequest<ProjectCreateKeyless>;

    public class  AddProjectHandler:IRequestHandler<AddProjectCommand, ProjectCreateKeyless>
    {
        private readonly AppDbContext dbContext;
        public  AddProjectHandler(AppDbContext _dbContext)
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
            dbContext.Project.Add(project);
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
