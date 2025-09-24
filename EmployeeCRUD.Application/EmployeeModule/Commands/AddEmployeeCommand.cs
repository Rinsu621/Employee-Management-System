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
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record AddEmployeeCommand(EmployeeDto employee) : IRequest<EmployeeResponseDto>;

    public class AddEmployeeHandler : IRequestHandler<AddEmployeeCommand, EmployeeResponseDto>
    {
        private readonly IAppDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AddEmployeeHandler(IAppDbContext _dbContext,
                              UserManager<ApplicationUser> _userManager,
                              RoleManager<IdentityRole> _roleManager)
        {
            dbContext = _dbContext;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public async Task<EmployeeResponseDto> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {  
            var entity = new  Employee
            {
                EmpName = request.employee.EmpName,
                Email = request.employee.Email,
                Phone = request.employee.Phone
            };
            dbContext.Employees.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            
            var user = new ApplicationUser
            {
                UserName = entity.Email,
                Email = entity.Email,
                EmployeeId = entity.Id
            };
            var defaultPassword = "Default@123";
            var result = await userManager.CreateAsync(user, defaultPassword);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (!await roleManager.RoleExistsAsync(request.employee.Role))
            {
                await roleManager.CreateAsync(new IdentityRole(request.employee.Role));
            }
            await userManager.AddToRoleAsync(user, request.employee.Role);



            return new EmployeeResponseDto
            {
                Id = entity.Id,
                EmpName = entity.EmpName,
                Email = entity.Email,
                Phone = entity.Phone,
                Role = request.employee.Role,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
