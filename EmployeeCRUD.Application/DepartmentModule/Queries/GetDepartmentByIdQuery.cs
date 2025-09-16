using Ardalis.GuardClauses;
using EmployeeCRUD.Application.Department.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Department.Queries
{
    public record GetDepartmentByIdQuery([property:FromRoute] Guid Id) : IRequest<DepartmentResultDto>;
    public class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentResultDto>
    {
        private readonly IAppDbContext dbContext;

        public GetDepartmentByIdHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DepartmentResultDto> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await dbContext.Departments.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            Guard.Against.Null(department, nameof(department), $"Department with Id '{request.Id}' not found.");
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
