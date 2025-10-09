using Ardalis.GuardClauses;
using EmployeeCRUD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.AuthModel.Commands
{
    public record ChangePasswordCommand(string currentPassword, string newPassword) : IRequest<bool>;
   
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ChangePasswordCommandHandler(UserManager<ApplicationUser> _userManager, IHttpContextAccessor _httpContextAccessor)
        {
            userManager = _userManager;
            httpContextAccessor = _httpContextAccessor;
        }
        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            //ClaimTypes.NameIdentifier maps to sub from wwhich we get user id
           var userId= httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guard.Against.NullOrEmpty(userId, "User not found");
            var user = await userManager.FindByIdAsync(userId);
            Guard.Against.Null(user, "User not found");

            var result= await userManager.ChangePasswordAsync(user, request.currentPassword, request.newPassword);

            if (!result.Succeeded)
                throw new InvalidOperationException("Password change failed");

            //Revoking the refresh token after password change
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
