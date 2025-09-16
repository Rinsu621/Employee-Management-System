using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRUD.Application.ProjectModule.Commands
{
    public record UpdateStatusCommand(Guid id, string status):IRequest<Project>;

    public class UpdateStatusHandler:IRequestHandler<UpdateStatusCommand, Project>
    {
        private readonly IAppDbContext dbContext;
        public UpdateStatusHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<Project> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == request.id, cancellationToken);

            project.Status = request.status;
            dbContext.Projects.Update(project);
            await dbContext.SaveChangesAsync(cancellationToken);

            return project;

        }
    }
 
}
