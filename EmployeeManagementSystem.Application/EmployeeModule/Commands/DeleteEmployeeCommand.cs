using Ardalis.GuardClauses;
using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManagementSystem.Application.EmployeeModule.Commands
{
  public record DeleteEmployeeCommand(Guid Id):IRequest<DeleteEmployeeResponse>;
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, DeleteEmployeeResponse>
    {
       
        private readonly IAppDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public DeleteEmployeeHandler(IAppDbContext _dbContext, UserManager<ApplicationUser> _userManager)
        {
           
            dbContext = _dbContext;
            userManager = _userManager;
        }
       
        public async Task<DeleteEmployeeResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                 {
                var existingEmployee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
                Guard.Against.Null(existingEmployee, nameof(existingEmployee), $"Employee with Id '{request.Id}' not found.");

                var user = await userManager.Users.FirstOrDefaultAsync(u => u.EmployeeId == existingEmployee.Id, cancellationToken);
                if (user != null)
                {
                    //delete in all identity table
                    var result= await userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                        throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                dbContext.Employees.Remove(existingEmployee);
                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return new DeleteEmployeeResponse
                {
                    Success = true,
                    Message = "Employee removed successfully."
                };
                }
                catch
                {
                await transaction.RollbackAsync(cancellationToken);
                throw;
                }
        }
    }
}
