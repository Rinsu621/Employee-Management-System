using EmployeeManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.SalaryModule.DTO
{
    public class SalaryResponseDto
    {
        public Guid Id { get; set; }
        public string EmpName { get; set; }

        public decimal BasicSalary { get; set; }
        public decimal Conveyance { get; set; }
        public decimal Tax { get; set; }
        public decimal PF { get; set; }
        public decimal ESI { get; set; }
        public string PaymentMode { get; set; }

        public string Status {  get; set; }
        public DateTime CreatedAt { get; set; }


        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
    }
}
