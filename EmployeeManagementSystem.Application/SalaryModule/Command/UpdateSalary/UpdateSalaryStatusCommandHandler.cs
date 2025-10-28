using Dapper;
using EmployeeManagementSystem.Application.Configuration;
using EmployeeManagementSystem.Application.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.SalaryModule.Command.UpdateSalary
{
    public class UpdateSalaryStatusCommandHandler : IRequestHandler<UpdateSalaryStatusCommand, bool>
    {
        private readonly IDbConnectionService _connectionService;
        private readonly DbSettings _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateSalaryStatusCommandHandler(IDbConnectionService connectionService, IOptions<DbSettings> options, IHttpContextAccessor httpContextAccessor)
        {
            _connectionService = connectionService;
            _settings = options.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(UpdateSalaryStatusCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new Exception("User Id not found in token");

            Guid ActionBy = Guid.Parse(userIdClaim.Value);
            using var connection = _connectionService.CreateConnection(_settings.SalaryConnection);

            var parameters = new DynamicParameters();
            parameters.Add("@Id", request.Id);
            parameters.Add("@Status", request.Status);
            parameters.Add("@ActionBy", ActionBy);
            

            var result = await connection.QueryFirstOrDefaultAsync<int>(
                "UpdateSalaryStatus",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result == 1;
        }
    }
}
