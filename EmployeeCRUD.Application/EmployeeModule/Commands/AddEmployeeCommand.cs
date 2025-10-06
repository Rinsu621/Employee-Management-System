using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record AddEmployeeCommand(EmployeeDto employee) : IRequest<EmployeeResponseDto>;

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
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var entity = new Employee
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
                var defaultPassword = configuration["DefaultPassword:Password"];
                var result = await userManager.CreateAsync(user, defaultPassword);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                await userManager.AddToRoleAsync(user, request.employee.Role);
                BackgroundJob.Enqueue(() => emailService.SendEmployeeCredentialsAsync(user.Email, defaultPassword));
                await transaction.CommitAsync(cancellationToken);


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
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
