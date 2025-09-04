using EmployeeCRUD.Application.Dtos.Departments;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Departments
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
            var department = await dbContext.Departments.FindAsync(new object[] { request.DepartmentId }, cancellationToken);
            if (department == null)
            {
                throw new KeyNotFoundException($"Department with Id '{request.DepartmentId}' not found.");
            }
            var employee = await dbContext.Employees.FindAsync(new object[] { request.EmployeeId }, cancellationToken);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.EmployeeId}' not found.");
            }
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
