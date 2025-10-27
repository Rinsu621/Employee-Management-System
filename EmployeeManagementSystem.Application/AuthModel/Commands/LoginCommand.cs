using Ardalis.GuardClauses;
using EmployeeManagementSystem.Application.AuthModel.Dto;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Application.AuthModel.Commands
{
    public record LoginCommand(string Email, string Password): IRequest<LoginResponseDto>;
    
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJwtService jwtService;
        private readonly IAppDbContext dbContext;

        public LoginCommandHandler(UserManager<ApplicationUser> _userManager, IJwtService _jwtService, IAppDbContext _dbContext)
        {
            userManager = _userManager;
            jwtService = _jwtService;
            dbContext = _dbContext;
        }
        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
           var user = await userManager.FindByEmailAsync(request.Email);
            //Guard.Against.Null(user, nameof(user), message: "Invalid Email or Password");
            Guard.Against.Null(user, nameof(user), "Invalid Email or Password");
            var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid Email or Password");

            var employee = await dbContext.Employees
                               .FirstOrDefaultAsync(e => e.Id == user.EmployeeId);

            string employeeName = employee != null ? employee.EmpName : user.UserName;

            var token = await jwtService.GenerateAccessToken(user);
            var refreshtoken = jwtService.GenerateRefreshToken();

            user.RefreshToken= refreshtoken;
            user.RefreshTokenExpiryTime= DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshtoken,
                Name = employeeName
            };
            

        }
    }
}
