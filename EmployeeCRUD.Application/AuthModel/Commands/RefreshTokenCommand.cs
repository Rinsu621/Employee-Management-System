using Ardalis.GuardClauses;
using EmployeeCRUD.Application.AuthModel.Dto;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.AuthModel.Commands
{
    public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<LoginResponseDto>;

    public class RefreshTokenCommandHandler:IRequestHandler<RefreshTokenCommand, LoginResponseDto>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJwtService jwtService;

        public RefreshTokenCommandHandler(UserManager<ApplicationUser> _userManager, IJwtService _jwtServices)
        {
            userManager = _userManager;
            jwtService = _jwtServices;
        }

        public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Guard.Against.NullOrEmpty(userId, nameof(userId),"Invalid Id");

            var user= await userManager.FindByIdAsync(userId);
            Guard.Against.Null(user, nameof(user),"Unauthorized user");

            if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token is invalid or revoked");

            var newToken = await jwtService.GenerateAccessToken(user);
            var newRefreshToken = jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            return new LoginResponseDto { Token = newToken, RefreshToken = newRefreshToken, Name = user.UserName };
        }

    }

}
