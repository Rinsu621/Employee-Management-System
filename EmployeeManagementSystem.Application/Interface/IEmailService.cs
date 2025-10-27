

namespace EmployeeManagementSystem.Application.Interface
{
    public interface IEmailService
    {
        Task SendEmployeeCredentialsAsync(string toEmail, string password);
    }
}
