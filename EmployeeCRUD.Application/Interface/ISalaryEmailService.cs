using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Interface
{
    public interface ISalaryEmailService
    {
        Task SendSalarySlipAsync(string toEmail, byte[] pdf, string attachmentName);

    }
}
