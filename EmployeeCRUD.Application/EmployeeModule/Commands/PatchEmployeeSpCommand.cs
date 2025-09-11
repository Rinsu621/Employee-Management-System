using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
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
    public record PatchEmployeeSpCommand(Guid Id,EmployeePatchDto employee) : IRequest<EmployeeUpdateResponse>;

    public class PatchEmployeeSpHandler : IRequestHandler<PatchEmployeeSpCommand, EmployeeUpdateResponse>

    {
        private readonly IAppDbContext dbContext;

        public PatchEmployeeSpHandler( IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<EmployeeUpdateResponse> Handle(PatchEmployeeSpCommand request, CancellationToken cancellationToken)
        { 

            var updatedEmployee = await dbContext.Employees
                .FromSqlInterpolated($"EXEC PatchEmployee @Id={request.Id}, @EmpName={request.employee.EmpName}, @Email={request.employee.Email}, @Phone={request.employee.Phone}")
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

            return  updatedEmployee;

        }

    }
}
