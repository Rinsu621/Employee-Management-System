using EmployeeCRUD.Application.Interface;
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
    public record AddProjectSpCommand(string ProjectName, string Description, DateTime StartDate, DateTime? EndDate, decimal Budget, string Status, string? ClientName) : IRequest<ProjectCreateKeyless>;

    public class AddProjectSpHandler : IRequestHandler<AddProjectSpCommand, ProjectCreateKeyless>
    {
        private readonly IAppDbContext dbContext;
        public AddProjectSpHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<ProjectCreateKeyless> Handle(AddProjectSpCommand request, CancellationToken cancellationToken)
        {
            var result = dbContext.ProjectCreateKeyless
                 .FromSqlInterpolated($"EXEC AddProject {request.ProjectName}, {request.Description}, {request.StartDate}, {request.EndDate}, {request.Budget}, {request.Status}, {request.ClientName}")
                 .AsNoTracking()
                 .AsEnumerable()
                 .FirstOrDefault();

            return result;
        }
    }
}
