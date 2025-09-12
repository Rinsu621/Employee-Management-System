using Ardalis.GuardClauses;
using EmployeeCRUD.Domain.Interface;
using EmployeeCRUD.Infrastructure.Data.keyless;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Commands
{
    public record AssignDepartmentSpCommand(Guid ProjectId, Guid DepartmentId) : IRequest<ProjectDepartmentResponse>;

    public class AssignDepartmentSpHandler: IRequestHandler<AssignDepartmentSpCommand, ProjectDepartmentResponse>
    {
        private readonly IAppDbContext dbContext;
        public AssignDepartmentSpHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<ProjectDepartmentResponse> Handle(AssignDepartmentSpCommand request, CancellationToken cancellationToken)
        {
            var result = dbContext.ProjectDepartmentResponses
                .FromSqlInterpolated($"EXEC AssignDepartment {request.ProjectId}, {request.DepartmentId}")
                .AsEnumerable()
                .FirstOrDefault();
            Guard.Against.Null(result, nameof(result), "Failed to assign department to project");
            return result;
        }
    }
}
