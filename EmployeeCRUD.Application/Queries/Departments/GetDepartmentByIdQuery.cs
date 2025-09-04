using EmployeeCRUD.Application.Dtos.Departments;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Queries.Departments
{
    public record GetDepartmentByIdQuery(Guid DepartmentId) : IRequest<DepartmentResultDto>;
    public class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentResultDto>
    {
        private readonly AppDbContext dbContext;

        public GetDepartmentByIdHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DepartmentResultDto> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await dbContext.Departments.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == request.DepartmentId, cancellationToken);

            if (department == null)
            {
                throw new KeyNotFoundException($"Department with Id '{request.DepartmentId}' was not found.");
            }

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
