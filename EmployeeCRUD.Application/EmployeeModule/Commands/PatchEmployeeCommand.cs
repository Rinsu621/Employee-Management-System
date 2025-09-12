using Ardalis.GuardClauses;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Domain.Interface;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record PatchEmployeeCommand(Guid Id, EmployeePatchDto Employee) : IRequest<EmployeeUpdateResponse>;

    public class PatchEmployeeHandler : IRequestHandler<PatchEmployeeCommand, EmployeeUpdateResponse>
    {
       
        private readonly Domain.Interface.IAppDbContext dbContext;
        public PatchEmployeeHandler(Domain.Interface.IAppDbContext _dbContext)
        {
           
            dbContext = _dbContext;
        }
        public async Task<EmployeeUpdateResponse> Handle(PatchEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee=await dbContext.Employees.FindAsync(request.Id);
            Guard.Against.Null(employee, nameof(employee), $"Employee with Id '{request.Id}' not found.");

            if (!string.IsNullOrEmpty(request.Employee.EmpName))
                employee.EmpName = request.Employee.EmpName;

            if (!string.IsNullOrEmpty(request.Employee.Email))
                employee.Email = request.Employee.Email;

            if (!string.IsNullOrEmpty(request.Employee.Phone))
                employee.Phone = request.Employee.Phone;

            employee.UpdatedAt = DateTime.UtcNow;

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
