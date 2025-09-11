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
    public record AddProjectSpCommand(string ProjectName, string Description, DateTime StartDate, DateTime? EndDate, decimal Budget, string Status, string? ClientName):IRequest<ProjectCreateKeyless>;

    public class AddProjectSpHandler:IRequestHandler<AddProjectSpCommand, ProjectCreateKeyless>
    {
        private readonly AppDbContext dbContext;
        public AddProjectSpHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<ProjectCreateKeyless> Handle(AddProjectSpCommand request, CancellationToken cancellationToken)
        {
            var result = dbContext.Set<ProjectCreateKeyless>()
                 .FromSqlInterpolated($"EXEC CreateProject @ProjectName={request.ProjectName}, @Description={request.Description}, @StartDate={request.StartDate}, @EndDate={request.EndDate}, @Budget={request.Budget}, @Status={request.Status}, @ClientName={request.ClientName}")
                 .AsNoTracking()
                 .AsEnumerable()
                 .FirstOrDefault();

            return  result;
        }
    }
}
