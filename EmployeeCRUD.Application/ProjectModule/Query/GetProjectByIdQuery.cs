
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

namespace EmployeeCRUD.Application.ProjectModule.Query
{
    public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectCreateKeyless>;

    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectCreateKeyless>
    {
        private readonly IAppDbContext dbContext;

        public GetProjectByIdHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<ProjectCreateKeyless> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {

            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == request.Id);

            return new ProjectCreateKeyless
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

        }
    }
}
