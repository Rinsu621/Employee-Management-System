using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Employees
{
   public record  PatchEmployeeCommand(Guid Id, EmployeePatchDto Employee) : IRequest<EmployeeUpdateResponse>;

    public class PatchEmployeeHandler : IRequestHandler<PatchEmployeeCommand, EmployeeUpdateResponse>
    {
       
        private readonly AppDbContext dbContext;
        public PatchEmployeeHandler( AppDbContext _dbContext)
        {
           
            dbContext = _dbContext;
        }
        public async Task<EmployeeUpdateResponse> Handle(PatchEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee=await dbContext.Employees.FindAsync(request.Id);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.Id}' not found.");
            }

            if (request.Employee.EmpName != null)
                employee.EmpName = request.Employee.EmpName;

            if (request.Employee.Email != null)
                employee.Email = request.Employee.Email;

            if (request.Employee.Phone != null)
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
