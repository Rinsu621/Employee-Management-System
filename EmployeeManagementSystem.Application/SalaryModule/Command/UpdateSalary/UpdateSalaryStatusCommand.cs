using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.SalaryModule.Command.UpdateSalary
{
    public record UpdateSalaryStatusCommand(Guid Id, string Status) : IRequest<bool>;
}
