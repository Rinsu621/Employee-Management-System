using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Application.SalaryModule.Document;
using EmployeeCRUD.Application.SalaryModule.DTO;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Domain.Enums;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.SalaryModule.Command.AddSalary
{
    public record AddSalaryCommand(Guid EmployeeId, decimal BasicSalary, decimal Conveyance, decimal Tax, decimal Pf, decimal ESI, string PaymentMethod, string Status, DateTime SalaryDate) : IRequest<Guid>;

   
}
