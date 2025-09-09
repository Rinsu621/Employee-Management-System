using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Exceptions;
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

namespace EmployeeCRUD.Application.Command.Employees
{
   public record UpdateEmployeeCommand(Guid Id,EmployeeDto Employee) : IRequest<EmployeeUpdateResponse>;

    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeUpdateResponse>
    {
       private readonly AppDbContext dbContext;
        public UpdateEmployeeHandler( AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<EmployeeUpdateResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {

            var employee = await dbContext.Employees.FindAsync(request.Id);
            employee.EmpName = request.Employee.EmpName;
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
