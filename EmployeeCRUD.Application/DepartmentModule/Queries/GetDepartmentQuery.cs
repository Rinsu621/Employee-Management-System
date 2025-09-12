using EmployeeCRUD.Application.Department.Dtos;
using EmployeeCRUD.Domain.Interface;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Department.Queries
{
    public record GetDepartmentQuery(): IRequest<IEnumerable<DepartmentResultDto>>;

    public class GetDepartmentQueryHandler : IRequestHandler<GetDepartmentQuery, IEnumerable<DepartmentResultDto>>
    {
        private readonly Domain.Interface.IAppDbContext dbContext;

        public GetDepartmentQueryHandler(Domain.Interface.IAppDbContext _dbContext)
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
