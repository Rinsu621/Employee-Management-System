using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Data.keyless;
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
    public record PatchEmployeeSpCommand(Guid Id, EmployeePatchDto employee) : IRequest<EmployeeUpdateKeyless>;

    public class PatchEmployeeSpHandler : IRequestHandler<PatchEmployeeSpCommand, EmployeeUpdateKeyless>

    {
        private readonly AppDbContext dbContext;

        public PatchEmployeeSpHandler( AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<EmployeeUpdateKeyless> Handle(PatchEmployeeSpCommand request, CancellationToken cancellationToken)
        { 

            var updatedEmployee =  dbContext.Set<EmployeeUpdateKeyless>()
                .FromSqlInterpolated($"EXEC PatchEmployee @Id={request.Id}, @EmpName={request.employee.EmpName}, @Email={request.employee.Email}, @Phone={request.employee.Phone}")
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault();

            if (updatedEmployee == null)
                throw new KeyNotFoundException($"Employee with Id '{request.Id}' not found.");

            return  updatedEmployee;

        }

    }
}
