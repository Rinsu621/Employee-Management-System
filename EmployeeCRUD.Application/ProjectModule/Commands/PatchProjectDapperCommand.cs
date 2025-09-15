using EmployeeCRUD.Application.ProjectModule.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Commands
{
    public record PatchProjectDapperCommand(Guid Id, string ProjectName, string? Description, DateTime? EndDate, decimal? Budget, string? Status, string? ClientName, Guid? ProjectManagerId, List<Guid>? TeamMemberIds) : IRequest<UpdateProjectDto>;
   
}
