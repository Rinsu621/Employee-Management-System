using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Data.keyless;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record UpdateEmployeeSpCommand( Guid Id,  EmployeeDto employee) : IRequest<EmployeeUpdateResponse>;

    public class UpdateEmployeeSpHandler : IRequestHandler<UpdateEmployeeSpCommand, EmployeeUpdateResponse>
    {
        private readonly AppDbContext dbContext;

        public UpdateEmployeeSpHandler( AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<EmployeeUpdateResponse> Handle(UpdateEmployeeSpCommand request, CancellationToken cancellationToken)
        {

            var updatedEmployee =await dbContext.Employees
                .FromSqlInterpolated($"EXEC UpdateEmployee @Id={request.Id}, @EmpName={request.employee.EmpName}, @Email={request.employee.Email}, @Phone={request.employee.Phone}")
                .AsNoTracking()
                .Select(x=> new EmployeeUpdateResponse
                {
                    Id = x.Id,
                    Name = x.EmpName,
                    Email = x.Email,
                    Phone = x.Phone,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);
            return updatedEmployee;
        }
    }
  }
