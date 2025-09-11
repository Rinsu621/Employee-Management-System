using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data.Keyless;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record AddEmployeeSPCommand(EmployeeDto employee) : IRequest<EmployeeResponseDto>;

    public class AddEmployeeSPHandler : IRequestHandler<AddEmployeeSPCommand, EmployeeResponseDto>
    {
        private readonly IAppDbContext dbContext;
       

        public AddEmployeeSPHandler(IAppDbContext _dbContext )
        {
            dbContext = _dbContext;
            
        }
        public async Task<EmployeeResponseDto> Handle(AddEmployeeSPCommand request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Employees
            .FromSqlInterpolated($"EXEC AddEmployee @EmpName={request.employee.EmpName}, @Email={request.employee.Email}, @Phone={request.employee.Phone}")
            .AsNoTracking()
            .Select(x => new EmployeeResponseDto
            {
                Id = x.Id,
                Name = x.EmpName,
                Email = x.Email,
                Phone = x.Phone,
                CreatedAt = x.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
