using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Commands
{
    public record DeleteProjectCommand(Guid Id):IRequest<Dele>;
    {
    }
}
