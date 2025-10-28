using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeManagementSystem.Application.Configuration;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Domain.Entities;
using Hangfire;
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

namespace EmployeeManagementSystem.Application.SalaryModule.Command.AddSalaryDapper
{
    public class AddSalaryDapperCommandHandler : IRequestHandler<AddSalaryDapperCommand, Guid>
    {
        private readonly IDbConnectionService connection;
        private readonly DbSettings _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddSalaryDapperCommandHandler(IDbConnectionService _connection, IOptions<DbSettings> options, IHttpContextAccessor httpContextAccessor)
        {
            connection =_connection;
            _settings = options.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> Handle(AddSalaryDapperCommand request, CancellationToken cancellationToken)
        {

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new Exception("User Id not found in token");

            Guid createdBy = Guid.Parse(userIdClaim.Value);


            var parameter = new DynamicParameters();
            parameter.Add("@Id", dbType: DbType.Guid, direction: ParameterDirection.Output);
            parameter.Add("@EmployeeId", request.EmployeeId);
            parameter.Add("@BasicSalary", request.BasicSalary);
            parameter.Add("@Conveyance", request.Conveyance);
            parameter.Add("@Tax", request.Tax);
            parameter.Add("@PF", request.Pf);
            parameter.Add("@ESI", request.ESI);
            parameter.Add("@PaymentMode", request.PaymentMethod);
            parameter.Add("@CreatedBy", createdBy);
            parameter.Add("@SalaryDate", request.SalaryDate);

            using var conn = connection.CreateConnection(_settings.SalaryConnection);
            await conn.ExecuteAsync(
                "AddSalary",
                parameter,
                commandType: CommandType.StoredProcedure
            );

            var response = parameter.Get<Guid>("@Id");
          
            BackgroundJob.Enqueue<IMediator>(mediator=> mediator.Send(new GenerateSalaryPdfCommand(response), cancellationToken));
            return response;

        }
    }
}
