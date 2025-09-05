using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
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
    public record AddEmployeeCommand(EmployeeDto employee) : IRequest<EmployeeResponseDto>;

    public class AddEmployeeHandler : IRequestHandler<AddEmployeeCommand, EmployeeResponseDto>
    {
        private readonly IValidator<EmployeeDto> validator;
        private readonly AppDbContext dbContext;

        public AddEmployeeHandler(IValidator<EmployeeDto> _validator, AppDbContext _dbContext)
        {
            validator = _validator;
            dbContext = _dbContext;
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

            var existingEmployee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email == request.employee.Email);

            if (existingEmployee != null)
            {
                throw new InvalidOperationException($"An employee with email '{request.employee.Email}' already exists.");
            }
            var entity = new Employee
            {
                EmpName = request.employee.EmpName,
                Email = request.employee.Email,
                Phone = request.employee.Phone,
                Department = null
            };

           dbContext.Employees.Add(entity);
          await dbContext.SaveChangesAsync(cancellationToken);

            return new EmployeeResponseDto
            {
                Id = entity.Id,
                Name = entity.EmpName,
                Email = entity.Email,
                Phone = entity.Phone,
                CreatedAt = entity.CreatedAt
            };



        }


    }
}
