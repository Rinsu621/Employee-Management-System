using EmployeeCRUD.Application.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Services
{
    public class SalaryEmailService:ISalaryEmailService
    {
        private readonly IConfiguration configuration;
        public SalaryEmailService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task SendSalarySlipAsync(string toEmail, byte[] pdf, string attachmentName)
        {
            var smtpHost = configuration["Smtp:Host"];
            var smtpPort = int.Parse(configuration["Smtp:Port"]);
            var smtpUser = configuration["Smtp:Username"];
            var smtpPass = configuration["Smtp:Password"];
            var fromEmail = configuration["Smtp:FromEmail"];

            var message = new MailMessage();
            message.To.Add(toEmail);
            message.Subject = "Salary Slip";
            message.Body = "Please find attached your salary slip.";
            message.IsBodyHtml = false;
            message.From = new MailAddress(fromEmail);

            if (pdf != null && attachmentName != null)
            {
                var ms = new System.IO.MemoryStream(pdf);
                message.Attachments.Add(new Attachment(ms, attachmentName));
            }

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }

}
