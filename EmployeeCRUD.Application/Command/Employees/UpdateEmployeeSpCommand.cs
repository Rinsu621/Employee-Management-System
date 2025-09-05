using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Data.keyless;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Employees
{
    public record UpdateEmployeeSpCommand(Guid Id, EmployeeDto employee) : IRequest<EmployeeUpdateKeyless>;

    public class UpdateEmployeeSpHandler : IRequestHandler<UpdateEmployeeSpCommand, EmployeeUpdateKeyless>
    {
        private readonly IValidator<EmployeeDto> validator;
        private readonly AppDbContext dbContext;

        public UpdateEmployeeSpHandler(IValidator<EmployeeDto> _validator, AppDbContext _dbContext)
        {
            validator = _validator;
            dbContext = _dbContext;
        }

        public async Task<EmployeeUpdateKeyless> Handle(UpdateEmployeeSpCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.employee, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(er => er.ErrorMessage).ToArray()
                    );

                throw new CustomValidationException(errors);
            }

            var updatedEmployee = dbContext.Set<EmployeeUpdateKeyless>()
                .FromSqlInterpolated($"EXEC UpdateEmployee @Id={request.Id}, @EmpName={request.employee.EmpName}, @Email={request.employee.Email}, @Phone={request.employee.Phone}")
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault();

            if (updatedEmployee == null)
                throw new KeyNotFoundException($"Employee with Id '{request.Id}' not found.");

            return updatedEmployee;
        }
    }
  }
