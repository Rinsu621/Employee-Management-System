using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record AddEmployeeCommand(string EmpName, string Email, string Phone, Guid?DepartmentId, string Role) : IRequest<EmployeeResponseDto>;

    public class AddEmployeeHandler : IRequestHandler<AddEmployeeCommand, EmployeeResponseDto>
    {
        private readonly IAppDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly IEmailService emailService;

        public AddEmployeeHandler(IAppDbContext _dbContext,
                              UserManager<ApplicationUser> _userManager,
                              RoleManager<IdentityRole> _roleManager,
                              IConfiguration _configuration,
                              IEmailService _emailService)
        {
            dbContext = _dbContext;
            userManager = _userManager;
            roleManager = _roleManager;
            configuration = _configuration;
            emailService = _emailService;
        }
        public async Task<EmployeeResponseDto> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            var defaultPassword = configuration["DefaultPassword:Password"];
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var entity = new Employee
                {
                    EmpName = request.EmpName,
                    Email = request.Email,
                    Phone = request.Phone,
                    DepartmentId= request.DepartmentId
                };
                dbContext.Employees.Add(entity);
                await dbContext.SaveChangesAsync(cancellationToken);
                var user = new ApplicationUser
                {
                    UserName = entity.Email,
                    Email = entity.Email,
                    EmployeeId = entity.Id
                };
              
                //creates an entry in AspNetUsers table
                var result = await userManager.CreateAsync(user, defaultPassword);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create user");
                }
                //Add in AspNetUserRoles table i.e user id and role id
                await userManager.AddToRoleAsync(user, request.Role);
                await transaction.CommitAsync(cancellationToken);
                BackgroundJob.Enqueue(() => emailService.SendEmployeeCredentialsAsync(user.Email, defaultPassword));

                var departmentName = await dbContext.Departments
                   .Where(d => d.Id == request.DepartmentId)
                   .Select(d => d.DeptName)
                   .FirstOrDefaultAsync(cancellationToken);
                return new EmployeeResponseDto
                {
                    Id = entity.Id,
                    EmpName = entity.EmpName,
                    Email = entity.Email,
                    Phone = entity.Phone,
                    DepartmentName = departmentName,
                    Role = request.Role,
                    CreatedAt = entity.CreatedAt
                };
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
