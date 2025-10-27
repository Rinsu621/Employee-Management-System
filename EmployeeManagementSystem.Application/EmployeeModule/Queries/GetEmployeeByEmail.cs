using Ardalis.GuardClauses;
using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.EmployeeModule.Queries
{
    public record GetEmployeeByEmail(string Email) : IRequest<EmployeeResponseDto>;
  public class GetEmployeeByEmailHandler : IRequestHandler<GetEmployeeByEmail, EmployeeResponseDto>
    {
        private readonly IAppDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public GetEmployeeByEmailHandler(IAppDbContext _dbContext, UserManager<ApplicationUser> _userManager)
        {
            dbContext = _dbContext;
            userManager = _userManager;
        }

        public async Task<EmployeeResponseDto> Handle(GetEmployeeByEmail request, CancellationToken cancellationToken)
        {

            var employee = await dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Email == request.Email, cancellationToken);

            if (employee == null)
                return null;

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == employee.Email, cancellationToken);

            string? role = null;
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                role = roles.FirstOrDefault();
            }

            return new EmployeeResponseDto
            {
                Id = employee.Id,
                EmpName = employee.EmpName,
                Email = employee.Email,
                Phone = employee.Phone,
                Role = role,
                DepartmentName = employee.Department?.DeptName,
                CreatedAt = employee.CreatedAt
            };
        }
    }
}
