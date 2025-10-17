using Dapper;
using EmployeeCRUD.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.SalaryModule.Command.AddSalaryDapper
{
    public record AddSalaryDapperCommand(Guid EmployeeId, decimal BasicSalary, decimal Conveyance, decimal Tax, decimal Pf, decimal ESI, string PaymentMethod, string Status, DateTime SalaryDate):IRequest<Guid>;
    
  
}
