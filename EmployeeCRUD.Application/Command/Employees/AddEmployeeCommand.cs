using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Application.Interfaces;
using EmployeeCRUD.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Employees
{
    public record AddEmployeeCommand(EmployeeDto employee) : IRequest<EmployeeResponseDto>;

    public class AddEmployeeHandler : IRequestHandler<AddEmployeeCommand, EmployeeResponseDto>
    {
        private readonly IValidator<EmployeeDto> validator;
        private readonly IEmployeeRepository employeeRepository;

        public AddEmployeeHandler(IValidator<EmployeeDto> _validator, IEmployeeRepository _employeeRepository)
        {
            validator = _validator;
            employeeRepository = _employeeRepository;
        }

        public async Task<EmployeeResponseDto> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
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

            var existingEmployee = await employeeRepository.GetEmployeeByEmail(request.employee.Email);

            if (existingEmployee != null)
            {
                throw new InvalidOperationException($"An employee with email '{request.employee.Email}' already exists.");
            }
            var entity = new Employee
            {
                EmpName = request.employee.EmpName,
                Email = request.employee.Email,
                Phone = request.employee.Phone,
            };

            var savedEmployee = await employeeRepository.AddEmployeeAsync(entity);
            return new EmployeeResponseDto
            {
                Id = savedEmployee.Id,
                Name = savedEmployee.EmpName,
                Email = savedEmployee.Email,
                Phone = savedEmployee.Phone,
                CreatedAt = savedEmployee.CreatedAt,
            };



        }


    }
}
