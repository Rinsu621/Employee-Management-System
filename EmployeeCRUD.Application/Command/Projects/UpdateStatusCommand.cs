using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Projects
{
    public record UpdateStatusCommand(Guid id, string status):IRequest<Project>;

    public class UpdateStatusHandler:IRequestHandler<UpdateStatusCommand, Project>
    {
        private readonly AppDbContext dbContext;
        public UpdateStatusHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<Project> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var project = await dbContext.Project.FirstOrDefaultAsync(p => p.Id == request.id, cancellationToken);

            project.Status = request.status;
            dbContext.Update(project);
            await dbContext.SaveChangesAsync(cancellationToken);

            return project;

        }
    }
 
}
