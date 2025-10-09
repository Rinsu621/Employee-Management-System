using Ardalis.GuardClauses;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Data.keyless;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record PatchEmployeeSpCommand(Guid Id,EmployeePatchDto employee) : IRequest<EmployeeUpdateKeyless>;

    public class PatchEmployeeSpHandler : IRequestHandler<PatchEmployeeSpCommand, EmployeeUpdateKeyless>

    {
        private readonly IAppDbContext dbContext;

        public PatchEmployeeSpHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<EmployeeUpdateKeyless> Handle(PatchEmployeeSpCommand request, CancellationToken cancellationToken)
        { 

            var updatedEmployee = dbContext.EmployeeUpdateKeyless
                .FromSqlInterpolated($"EXEC PatchEmployee {request.Id}, {request.employee.EmpName}, {request.employee.Email}, {request.employee.Phone}")
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault();

            Guard.Against.Null(updatedEmployee, nameof(updatedEmployee), $"Employee with {request.Id} not found");

            return updatedEmployee;

        }

    }
}
