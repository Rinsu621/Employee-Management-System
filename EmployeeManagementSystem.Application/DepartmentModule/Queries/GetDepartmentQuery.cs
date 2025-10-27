using EmployeeManagementSystem.Application.Department.Dtos;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.Department.Queries
{
    public record GetDepartmentQuery(): IRequest<IEnumerable<DepartmentResultDto>>;

    public class GetDepartmentQueryHandler : IRequestHandler<GetDepartmentQuery, IEnumerable<DepartmentResultDto>>
    {
        private readonly IAppDbContext dbContext;

        public GetDepartmentQueryHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<IEnumerable<DepartmentResultDto>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
        {
            var departments = await dbContext.Departments
                .Include(d => d.Employees) 
                .ToListAsync(cancellationToken);

            return departments.Select(d => new DepartmentResultDto
            {
                Id = d.Id,
                Name = d.DeptName,
                Employees = d.Employees?.Select(e => new EmployeeReturnForDepartmentDto
                {
                    Id = e.Id,
                    Name = e.EmpName,
                    Email = e.Email,
                    Phone = e.Phone
                }).ToList() ?? new List<EmployeeReturnForDepartmentDto>() 
            });
        }

    }
   
}
