using Ardalis.GuardClauses;
using EmployeeCRUD.Application.Department.Dtos;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Department.Command
{
   public record AddEmployeeToDepartmentCommand(Guid DepartmentId, Guid EmployeeId):IRequest<DepartmentResultDto>;

    public class AddEmployeeToDepartmentHandler : IRequestHandler<AddEmployeeToDepartmentCommand, DepartmentResultDto>
    {
        private readonly AppDbContext dbContext;

        public AddEmployeeToDepartmentHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DepartmentResultDto> Handle(AddEmployeeToDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == request.DepartmentId, cancellationToken);
            Guard.Against.Null(department, nameof(department), $"Department with Id '{request.DepartmentId}' not found.");

            var employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);
            Guard.Against.Null(employee, nameof(employee),$"Employee with Id '{request.EmployeeId}' not found.");

            employee.DepartmentId = department.Id;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new DepartmentResultDto
            {
                Id = department.Id,
                Name = department.DeptName,
                Employees = dbContext.Employees
                    .Where(e => e.DepartmentId == department.Id)
                    .Select(e => new EmployeeReturnForDepartmentDto
                    {
                        Id = e.Id,
                        Name = e.EmpName,
                        Email = e.Email,
                        Phone = e.Phone
                    }).ToList()
            };
        }
    }
}
