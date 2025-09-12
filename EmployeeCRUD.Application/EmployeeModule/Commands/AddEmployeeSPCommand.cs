using Ardalis.GuardClauses;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Domain.Interface;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Data.Keyless;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record AddEmployeeSPCommand(EmployeeDto employee) : IRequest<EmployeeResponseKeyless>;

    public class AddEmployeeSPHandler : IRequestHandler<AddEmployeeSPCommand, EmployeeResponseKeyless>
    {
        private readonly Domain.Interface.IAppDbContext dbContext;
       

        public AddEmployeeSPHandler(Domain.Interface.IAppDbContext _dbContext )
        {
            dbContext = _dbContext;
            
        }
        public async Task<EmployeeResponseKeyless> Handle(AddEmployeeSPCommand request, CancellationToken cancellationToken)
        {
            var result =  dbContext.EmployeeResponseKeyless
            .FromSqlInterpolated($"EXEC AddEmployee {request.employee.EmpName},{request.employee.Email}, {request.employee.Phone}")
            .AsNoTracking()
            .AsEnumerable()
            .FirstOrDefault();
             Guard.Against.Null(result, nameof(result), "Failed to add employee");
            return result;
        }
    }
}
