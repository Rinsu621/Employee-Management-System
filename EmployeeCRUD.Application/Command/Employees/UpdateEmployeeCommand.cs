using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Application.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Employees
{
   public record UpdateEmployeeCommand(Guid Id, EmployeeDto employee) : IRequest<EmployeeUpdateResponse>;

    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeUpdateResponse>
    {
        private readonly IValidator<EmployeeDto> validator;
        private readonly IEmployeeRepository employeeRepository;
        public UpdateEmployeeHandler(IValidator<EmployeeDto> _validator, IEmployeeRepository _employeeRepository)
        {
            validator = _validator;
            employeeRepository = _employeeRepository;
        }
        public async Task<EmployeeUpdateResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
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

            var employee = await employeeRepository.GetEmployeeByIdAsync(request.Id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.Id}' not found.");
            }

            employee.EmpName = request.employee.EmpName;
            employee.Email = request.employee.Email;
             employee.Phone = request.employee.Phone;

            await employeeRepository.UpdateEmployeesAsync(employee);
            return new EmployeeUpdateResponse
            {
                Id = employee.Id,
                Name = employee.EmpName,
                Email = employee.Email,
                Phone = employee.Phone,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = DateTime.UtcNow

            };
        }
    }
}
