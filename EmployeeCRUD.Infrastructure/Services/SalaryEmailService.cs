using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Application.SalaryModule.DTO;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Services
{
    public class SalaryEmailService : ISalaryEmailService
    {
        private readonly IConfiguration configuration;
        public SalaryEmailService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task SendSalarySlipAsync(string toEmail, byte[] pdf, string attachmentName, SalaryPdfModel salary)
        {
            var smtpHost = configuration["Smtp:Host"];
            var smtpPort = int.Parse(configuration["Smtp:Port"]);
            var smtpUser = configuration["Smtp:Username"];
            var smtpPass = configuration["Smtp:Password"];
            var fromEmail = configuration["Smtp:FromEmail"];

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail, "Rinsu Pradhan"),
                Subject = "Salary Slip",
                IsBodyHtml = true,
                Body = GenerateEmailBody(salary)
            };

            message.To.Add(toEmail);

            // Attach PDF
            if (pdf != null && attachmentName != null)
            {
                var ms = new System.IO.MemoryStream(pdf);
                var attachment = new Attachment(ms, attachmentName);
                attachment.ContentStream.Position = 0;
                message.Attachments.Add(attachment);
            }

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            await client.SendMailAsync(message);

            // Dispose streams
            foreach (var attachment in message.Attachments)
                attachment.ContentStream.Dispose();
        }

        private string GenerateEmailBody(SalaryPdfModel salary)
        {
            return $@"
<html>
<head>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'/>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f8f9fa;
            color: #333;
            margin: 0;
            padding: 0;
        }}
        .container {{
            background-color: #fff;
            width: 95%;
            max-width: 600px;
            margin: 10px auto;
            padding: 15px;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.05);
        }}
        h2 {{
            color: #007bff;
            text-align: center;
            font-size: 20px;
            margin-bottom: 5px;
        }}
        h4 {{
            text-align: center;
            color: #555;
            margin: 0 0 15px 0;
            font-size: 16px;
        }}
        table {{
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }}
        th, td {{
            border: 1px solid #ddd;
            padding: 6px 8px;
            font-size: 13px;
            word-break: break-word;
        }}
        th {{
            background-color: #f1f1f1;
        }}
        .section-title {{
            background-color: #f8f9fa;
            padding: 8px;
            font-weight: bold;
            color: #007bff;
            border: 1px solid #ddd;
            border-bottom: none;
            font-size: 14px;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 11px;
            color: #666;
            text-align: center;
            font-style: italic;
        }}
        @media only screen and (max-width: 600px) {{
            .container {{
                width: 90% !important;
                padding: 10px !important;
            }}
            table, th, td {{
                font-size: 12px !important;
            }}
            h2 {{
                font-size: 18px !important;
            }}
            h4 {{
                font-size: 14px !important;
            }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>My Company Pvt. Ltd.</h2>
        <h4>Salary Slip for {salary.SalaryMonth:MM/yyyy}</h4>

        <div class='section-title'>Employee Information</div>
        <table>
            <tr>
                <th>Name</th><td>{salary.EmployeeName}</td>
                <th>Department</th><td>{salary.Department ?? "-"}</td>
            </tr>
            <tr>
                <th>Role</th><td>{salary.Role ?? "-"}</td>
                <th>Joined</th><td>{salary.Joined}</td>
            </tr>
            <tr>
                <th>Payment Method</th><td>{salary.PaymentMode ?? "-"}</td>
                <th>Status</th><td>{salary.Status}</td>
            </tr>
        </table>

        <div class='section-title'>Earnings & Deductions</div>
        <table>
            <tr>
                <th>S.No</th><th>Earning</th><th>Amount</th>
                <th>S.No</th><th>Deduction</th><th>Amount</th>
            </tr>
            <tr>
                <td>1</td><td>Basic Salary</td><td>{salary.BasicSalary:0.00}</td>
                <td>1</td><td>Professional Tax</td><td>{salary.Tax:0.00}</td>
            </tr>
            <tr>
                <td>2</td><td>Conveyance</td><td>{salary.Conveyance:0.00}</td>
                <td>2</td><td>PF</td><td>{salary.PF:0.00}</td>
            </tr>
            <tr>
                <td></td><td><b>Gross Salary</b></td><td><b>{salary.GrossSalary:0.00}</b></td>
                <td>3</td><td>ESI</td><td>{salary.ESI:0.00}</td>
            </tr>
            <tr>
                <td colspan='3'></td>
                <td></td><td><b>Total Deduction</b></td><td><b>{salary.Tax + salary.PF + salary.ESI:0.00}</b></td>
            </tr>
        </table>

        <div class='section-title'>Net Salary</div>
        <table>
            <tr>
                <th>Net Salary (NPR)</th>
                <td><b>{salary.NetSalary:0.00}</b></td>
            </tr>
        </table>

        <div class='footer'>
            This is a computer-generated salary slip. Please do not reply to this email.
        </div>
    </div>
</body>
</html>";
        }

    }
}
