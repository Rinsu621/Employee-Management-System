
using EmployeeCRUD.Domain.Common;
using EmployeeCRUD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Domain.Entities
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
        public SalaryStatus Status { get; set; }= SalaryStatus.Unpaid;

        public decimal GrossSalary => BasicSalary+Conveyance;

        public decimal NetSalary => BasicSalary - (Tax + PF+ ESI);

    }
}
