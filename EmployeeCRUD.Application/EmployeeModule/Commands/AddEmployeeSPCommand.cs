using Ardalis.GuardClauses;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Data.Keyless;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
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
