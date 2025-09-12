using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Domain.Interface;
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
    public record UpdateEmployeeSpCommand( Guid Id,  EmployeeDto employee) : IRequest<EmployeeUpdateKeyless>;

    public class UpdateEmployeeSpHandler : IRequestHandler<UpdateEmployeeSpCommand, EmployeeUpdateKeyless>
    {
        private readonly Domain.Interface.IAppDbContext dbContext;

        public UpdateEmployeeSpHandler(Domain.Interface.IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<EmployeeUpdateKeyless> Handle(UpdateEmployeeSpCommand request, CancellationToken cancellationToken)
        {

            var updatedEmployee = dbContext.EmployeeUpdateKeyless
                .FromSqlInterpolated($"EXEC UpdateEmployee {request.Id}, {request.employee.EmpName}, {request.employee.Email}, {request.employee.Phone}")
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault();

            return updatedEmployee;
        }
    }
  }
