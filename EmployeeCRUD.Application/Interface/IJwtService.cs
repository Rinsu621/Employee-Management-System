using EmployeeCRUD.Domain.Entities;
using System.Security.Claims;


namespace EmployeeCRUD.Application.Interface
{
    public interface IJwtService
    {
        Task<string> GenerateAccessToken(ApplicationUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
