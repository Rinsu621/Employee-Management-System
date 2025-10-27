using EmployeeManagementSystem.Application.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task SendEmployeeCredentialsAsync(string toEmail, string password)
        {
            var smtpHost = configuration["Smtp:Host"];
            var smtpPort = int.Parse(configuration["Smtp:Port"]);
            var smtpUser = configuration["Smtp:Username"];
            var smtpPass = configuration["Smtp:Password"];
            var fromEmail = configuration["Smtp:FromEmail"];

            var message = new MailMessage();
            message.To.Add(toEmail);
            message.Subject = "Your Credentials";
            message.Body = $"Your account has been created.\nEmail: {toEmail}\nPassword: {password}\nPlease change your password after logging in.";
            message.IsBodyHtml = false;
            message.From = new MailAddress(fromEmail);

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }
}
