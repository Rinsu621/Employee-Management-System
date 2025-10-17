using EmployeeCRUD.Application.SalaryModule.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.SalaryModule.Queries.GetSalaryDapper
{
    public record GetSalaryDetailQuery:IRequest<IEnumerable<SalaryResponseDto>>;
}
