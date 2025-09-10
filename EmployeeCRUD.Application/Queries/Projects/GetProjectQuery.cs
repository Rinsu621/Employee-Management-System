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
    public record GetProjectQuery:IRequest<IEnumerable<ProjectCreateKeyless>>;

    public class GetProjectHandler : IRequestHandler<GetProjectQuery, IEnumerable<ProjectCreateKeyless>>
    {
        private readonly AppDbContext dbContext;
        public GetProjectHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<IEnumerable<ProjectCreateKeyless>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var project= await dbContext.Project.ToListAsync(cancellationToken);

            return project.Select(p => new ProjectCreateKeyless
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Budget = p.Budget,
                Status = p.Status,
                ClientName = p.ClientName
            });



        }
    }
}
