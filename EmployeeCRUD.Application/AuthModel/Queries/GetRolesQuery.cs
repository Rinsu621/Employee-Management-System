using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.AuthModel.Queries
{
    public record GetRolesQuery() : IRequest<List<string>>;

    public class GetRolesHandler : IRequestHandler<GetRolesQuery, List<string>>
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public GetRolesHandler(RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
        }

        public async Task<List<string>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await Task.FromResult(roleManager.Roles.Select(r => r.Name).ToList());
            return roles;
        }
    }
}
