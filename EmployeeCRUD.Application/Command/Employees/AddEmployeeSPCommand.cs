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
        private readonly IValidator<EmployeeDto> validator;

        public AddEmployeeSPHandler(AppDbContext _dbContext, IValidator<EmployeeDto> _validator)
        {
            dbContext = _dbContext;
            validator = _validator;
        }

        public async Task<EmployeeResponseKeyless> Handle(AddEmployeeSPCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.employee, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new CustomValidationException(errors);
            }

            var employeeKeyless = dbContext.Set<EmployeeResponseKeyless>()
            .FromSqlInterpolated($"EXEC AddEmployee @EmpName={request.employee.EmpName}, @Email={request.employee.Email}, @Phone={request.employee.Phone}")
            .AsNoTracking()   //Donot track return entities
            .AsEnumerable()   //IQuerable to IEnumerable  , execute, pull the result ot memory and LINQ
            .FirstOrDefault();

            return employeeKeyless;
        }
    }
}
