using Ardalis.GuardClauses;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Infrastructure.Data.Keyless;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManagementSystem.Application.EmployeeModule.Commands
{
    public record AddEmployeeSPCommand(string EmpName, string Email, string Phone) : IRequest<EmployeeResponseKeyless>;

    public class AddEmployeeSPHandler : IRequestHandler<AddEmployeeSPCommand, EmployeeResponseKeyless>
    {
        private readonly IAppDbContext dbContext;
       

        public AddEmployeeSPHandler(IAppDbContext _dbContext )
        {
            dbContext = _dbContext;
            
        }
        public async Task<EmployeeResponseKeyless> Handle(AddEmployeeSPCommand request, CancellationToken cancellationToken)
        {
            var newId = Guid.NewGuid();
            var result = dbContext.Set<EmployeeResponseKeyless>()
                .FromSqlInterpolated($"EXEC AddEmployee @Id={newId}, @EmpName={request.EmpName}, @Email={request.Email}, @Phone={request.Phone}")
                .AsNoTracking()
                .AsEnumerable()
                .Single();
            Guard.Against.Null(result, nameof(result), "Failed to add employee");
            return result;
        }
    }
}
