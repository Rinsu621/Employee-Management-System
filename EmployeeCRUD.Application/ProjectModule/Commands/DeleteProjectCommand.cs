using Ardalis.GuardClauses;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Application.ProjectModule.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Commands
{
    public record DeleteProjectCommand(Guid Id):IRequest<DeleteProjectDto>;
    
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, DeleteProjectDto>
    {
        private readonly IAppDbContext dbContext;
        public DeleteProjectHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<DeleteProjectDto> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = dbContext.Projects.Where(p => p.Id == request.Id).SingleOrDefault();
            Guard.Against.Null(project, nameof(project), $"Project with Id '{request.Id}' not found.");
            dbContext.Projects.Remove(project);
            await dbContext.SaveChangesAsync(cancellationToken);


            return new DeleteProjectDto 
            {
                Success = true,
                Message = "Project removed successfully."
            };


        }
    }
}
