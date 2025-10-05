using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Queries
{
    public record GetAllEmployeesQuery(int page, int PageSize, string? Role=null, Guid? DepartmentId=null, DateTime? FromDate=null, DateTime? ToDate=null) : IRequest<EmployeePagedResponseDto>;

    public class GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesQuery, EmployeePagedResponseDto>
    {
        private readonly IAppDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public GetAllEmployeesHandler(IAppDbContext _dbContext, UserManager<ApplicationUser> _userManager)
        {
            dbContext = _dbContext;
            userManager = _userManager;
        }

        public async Task<EmployeePagedResponseDto> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employeeQuery = dbContext.Employees
                .Include(e => e.Department)
                .AsQueryable();

            if (request.DepartmentId.HasValue)
                employeeQuery = employeeQuery.Where(e => e.DepartmentId == request.DepartmentId.Value);

            if (request.FromDate.HasValue)
                employeeQuery = employeeQuery.Where(e => e.CreatedAt >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                employeeQuery = employeeQuery.Where(e => e.CreatedAt <= request.ToDate.Value);

            if (!string.IsNullOrEmpty(request.Role))
            {
                var userIdsWithRole = await userManager.GetUsersInRoleAsync(request.Role);
                var emailsWithRole = userIdsWithRole.Select(u => u.Email).ToList();
                employeeQuery = employeeQuery.Where(e => emailsWithRole.Contains(e.Email));
            }

            var filteredTotal = await employeeQuery.CountAsync(cancellationToken);

            var employees = await employeeQuery
                .OrderBy(e => e.CreatedAt)
                .Skip((request.page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var result = new List<EmployeeResponseDto>();

            foreach (var e in employees)
            {
                var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == e.Email, cancellationToken);
                var roles = user != null ? (await userManager.GetRolesAsync(user)).ToList() : new List<string>();

                result.Add(new EmployeeResponseDto
                {
                    Id = e.Id,
                    EmpName = e.EmpName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DepartmentName = e.Department?.DeptName,
                    Role = roles.FirstOrDefault() ?? "N/A",
                    CreatedAt = e.CreatedAt
                });
            }

            return new EmployeePagedResponseDto
            {
                TotalCount = filteredTotal,
                Employees = result
            };
        }
    }
}



