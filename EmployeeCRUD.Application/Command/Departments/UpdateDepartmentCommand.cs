using EmployeeCRUD.Application.Dtos.Departments;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Departments
{
    public record UpdateDepartmentCommand(Guid DepartmentId, DepartmentCreateDto department): IRequest<DepartmentResultDto>;

    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, DepartmentResultDto>
    {
        private readonly AppDbContext dbContext;

        public UpdateDepartmentHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<DepartmentResultDto> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department= await dbContext.Departments.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == request.DepartmentId, cancellationToken);

            if (department == null)
            {
                throw new KeyNotFoundException($"Department with Id '{request.DepartmentId}' not found.");
            }
            var exists = await dbContext.Departments
                .AnyAsync(d => d.DeptName == request.department.DeptName && d.Id != request.DepartmentId, cancellationToken);

            if (exists)
            {
                throw new AlreadyExistsException($"Department name '{request.department.DeptName}' already exists.");
            }
            department.DeptName = request.department.DeptName;

            await dbContext.SaveChangesAsync(cancellationToken);

            return new DepartmentResultDto
            {
                Id = department.Id,
                Name = department.DeptName,
                Employees = department.Employees?.Select(e => new EmployeeReturnForDepartmentDto
                {
                    Id = e.Id,
                    Name = e.EmpName,
                    Email = e.Email,
                    Phone = e.Phone
                }).ToList() ?? new List<EmployeeReturnForDepartmentDto>()
            };
        }
    }

}
