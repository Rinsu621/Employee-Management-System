using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.SalaryModule.DTO
{
    public class SalaryResponseDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }

        public decimal BasicSalary { get; set; }
        public decimal Conveyance { get; set; }
        public decimal Tax { get; set; }
        public decimal PF { get; set; }
        public decimal ESI { get; set; }
        public string PaymentMode { get; set; }

        public DateTime CreatedAt { get; set; }


        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
    }
}
