using Ardalis.GuardClauses;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Guards;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
   public record UpdateEmployeeCommand(Guid Id,EmployeeDto Employee) : IRequest<EmployeeUpdateResponse>;

    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeUpdateResponse>
    {
       private readonly IAppDbContext dbContext;
        public UpdateEmployeeHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<EmployeeUpdateResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {

            var employee = await dbContext.Employees.FindAsync(request.Id);
            Guard.Against.Null(employee, nameof(employee), $"Employee with Id '{request.Id}' not found.");

            employee.EmpName= request.Employee.EmpName;
            employee.Email = request.Employee.Email;
            employee.Phone = request.Employee.Phone;

            dbContext.Employees.Update(employee);
            await dbContext.SaveChangesAsync(cancellationToken);
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
