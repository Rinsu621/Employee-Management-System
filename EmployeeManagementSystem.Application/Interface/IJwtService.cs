using EmployeeManagementSystem.Domain.Entities;
using System.Security.Claims;


namespace EmployeeManagementSystem.Application.Interface
{
    public interface IJwtService
    {
        Task<string> GenerateAccessToken(ApplicationUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
