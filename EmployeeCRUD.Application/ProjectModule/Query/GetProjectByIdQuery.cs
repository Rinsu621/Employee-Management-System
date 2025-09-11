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

namespace EmployeeCRUD.Application.ProjectModule.Query
{
    public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectCreateDto>;

    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectCreateDto>
    {
        private readonly IAppDbContext dbContext;

        public GetProjectByIdHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<ProjectCreateDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {

            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == request.Id);

            return new ProjectCreateDto
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
