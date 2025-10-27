using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Infrastructure.Data;
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

namespace EmployeeManagementSystem.Application.EmployeeModule.Queries
{
    public record GetAllEmployeesQuery(int page, int PageSize, string? Role=null, Guid? DepartmentId=null, DateTime? FromDate=null, DateTime? ToDate=null, string? SearchTerm = null, string? SortKey=null, bool SortAsc=true) : IRequest<EmployeePagedResponseDto>;
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
            var employeeQuery = dbContext.Employees.Include(e => e.Department).Include(e=>e.User).AsQueryable();
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
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                var allUsers = await userManager.Users.ToListAsync(cancellationToken);
                var usersMatchingRole = new List<string>();
                foreach (var u in allUsers)
                {
                    var roles = await userManager.GetRolesAsync(u);
                    if (roles.Any(r => r.ToLower().Contains(term)))
                        usersMatchingRole.Add(u.Email);
                }
                employeeQuery = employeeQuery.Where(e =>
                    e.Id.ToString().Contains(term) ||
                    e.EmpName.ToLower().Contains(term) ||
                    e.Email.ToLower().Contains(term)||
                    (e.Department != null && e.Department.DeptName.ToLower().Contains(term)) ||
                    usersMatchingRole.Contains(e.Email)
                );
            }

            if (!string.IsNullOrEmpty(request.SortKey))
            {
                switch (request.SortKey.ToLower())
                {
                    case "empname":
                        employeeQuery = request.SortAsc
                            ? employeeQuery.OrderBy(e => e.EmpName)
                            : employeeQuery.OrderByDescending(e => e.EmpName);
                        break;
                    case "email":
                        employeeQuery = request.SortAsc
                            ? employeeQuery.OrderBy(e => e.Email)
                            : employeeQuery.OrderByDescending(e => e.Email);
                        break;
                    case "department":
                        employeeQuery = request.SortAsc
                            ? employeeQuery.OrderBy(e => e.Department.DeptName)
                            : employeeQuery.OrderByDescending(e => e.Department.DeptName);
                        break;
                    case "createdat":
                    default:
                        employeeQuery = request.SortAsc
                            ? employeeQuery.OrderBy(e => e.CreatedAt)
                            : employeeQuery.OrderByDescending(e => e.CreatedAt);
                        break;
                }
            }
            else
            {
                employeeQuery = employeeQuery.OrderBy(e => e.CreatedAt); // default
            }

            var filteredTotal = await employeeQuery.CountAsync(cancellationToken);
            var employees = await employeeQuery
           
                .Skip((request.page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            var result = new List<EmployeeResponseDto>();
            foreach (var e in employees)
            {
                var roles = e.User != null
                ? (await userManager.GetRolesAsync(e.User)).ToList()
                : new List<string>();
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



