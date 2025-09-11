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
    public record AddProjectSpCommand(string ProjectName, string Description, DateTime StartDate, DateTime? EndDate, decimal Budget, string Status, string? ClientName):IRequest<ProjectCreateDto>;

    public class AddProjectSpHandler:IRequestHandler<AddProjectSpCommand, ProjectCreateDto>
    {
        private readonly IAppDbContext dbContext;
        public AddProjectSpHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<ProjectCreateDto> Handle(AddProjectSpCommand request, CancellationToken cancellationToken)
        {
            var result = dbContext.Projects
                 .FromSqlInterpolated($"EXEC CreateProject {request.ProjectName}, {request.Description}, {request.StartDate}, {request.EndDate}, {request.Budget}, {request.Status}, {request.ClientName}")
                 .AsNoTracking()
                 .Select(p=> new ProjectCreateDto
                 {
                     Id = p.Id,
                     ProjectName = p.ProjectName,
                     Description = p.Description,
                     StartDate = p.StartDate,
                     EndDate = p.EndDate,
                     Budget = p.Budget,
                     Status = p.Status,
                     ClientName = p.ClientName
                 })
                 .FirstOrDefault();

            return  result;
        }
    }
}
