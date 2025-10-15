using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.SalaryModule.DTO
{
    public class SalaryPdfModel
    {
        public string EmployeeName { get; set; } = ""; // Add this
        public string Department { get; set; }

        public string Joined { get; set; }

        public string Role { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Conveyance { get; set; }
        public decimal Tax { get; set; }
        public decimal PF { get; set; }
        public decimal ESI { get; set; }
        public string PaymentMode { get; set; } = "";
        public string Status { get; set; } = "";
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public string SalaryMonth { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
