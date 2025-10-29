
using EmployeeManagementSystem.Domain.Common;
using EmployeeManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Domain.Entities
{
    public class Salary:BaseEntity
    {
        public Guid EmployeeId { get; set; }

        public decimal BasicSalary { get; set; }
        public decimal Conveyance { get; set; }
        public decimal Tax { get; set; }
        public decimal PF { get; set; }
        public decimal ESI { get; set; }
        public PaymentMethod PaymentMode { get; set; }
        public SalaryStatus Status { get; set; }= SalaryStatus.Pending;

        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ActionBy { get; set; }
        public DateTime? ActionAt { get; set; }
        public decimal GrossSalary => BasicSalary+Conveyance ;

        public decimal NetSalary => GrossSalary - (Tax + PF+ ESI);
        public DateTime SalaryDate { get; set; }


    }
}
