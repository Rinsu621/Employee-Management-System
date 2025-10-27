using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using EmployeeManagementSystem.Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace EmployeeManagementSystem.Application.EmployeeModule.Commands
{
    public record DeleteEmployeeSpCommand(Guid Id):IRequest<DeleteEmployeeResponse>;

    public class DeleteEmployeeSpHandler:IRequestHandler<DeleteEmployeeSpCommand, DeleteEmployeeResponse>
    {
        private readonly IAppDbContext dbContext;

        public DeleteEmployeeSpHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DeleteEmployeeResponse> Handle(DeleteEmployeeSpCommand request, CancellationToken cancellationToken)
        {
          
            var rowAffected = await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteEmployee @Id = {request.Id}");

            return new DeleteEmployeeResponse
            {
                Success = true,
                Message = "Employee removed successfully."
            };
        }
    }
}
