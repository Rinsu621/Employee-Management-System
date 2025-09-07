using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Data.Keyless;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Employees
{
    public record AddEmployeeSPCommand(EmployeeDto employee) : IRequest<EmployeeResponseKeyless>;

    public class AddEmployeeSPHandler : IRequestHandler<AddEmployeeSPCommand, EmployeeResponseKeyless>
    {
        private readonly AppDbContext dbContext;
       

        public AddEmployeeSPHandler(AppDbContext _dbContext )
        {
            dbContext = _dbContext;
            
        }

        public async Task<EmployeeResponseKeyless?> Handle(AddEmployeeSPCommand request, CancellationToken cancellationToken)
        {
            var result = dbContext.Set<EmployeeResponseKeyless>()
            .FromSqlInterpolated($"EXEC AddEmployee @EmpName={request.employee.EmpName}, @Email={request.employee.Email}, @Phone={request.employee.Phone}")
            .AsNoTracking()
            .AsEnumerable()
            .FirstOrDefault();

            return await Task.FromResult(result);
        }
    }
}
