using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeCRUD.Application.Configuration;
using EmployeeCRUD.Application.Interface;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.SalaryModule.Command.AddSalaryDapper
{
    public class AddSalaryDapperCommandHandler : IRequestHandler<AddSalaryDapperCommand, Guid>
    {
        private readonly IDbConnectionService connection;
        private readonly DbSettings _settings;
        public AddSalaryDapperCommandHandler(IDbConnectionService _connection, IOptions<DbSettings> options)
        {
            connection =_connection;
            _settings = options.Value;
        }

        public async Task<Guid> Handle(AddSalaryDapperCommand request, CancellationToken cancellationToken)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@EmployeeId", request.EmployeeId);
            parameter.Add("@BasicSalary", request.BasicSalary);
            parameter.Add("@Conveyance", request.Conveyance);
            parameter.Add("@Tax", request.Tax);
            parameter.Add("@PF", request.Pf);
            parameter.Add("@ESI", request.ESI);
            parameter.Add("@PaymentMethod", request.PaymentMethod);
            parameter.Add("@Status", request.Status);
            parameter.Add("@SalaryDate", request.SalaryDate);

            using var conn = connection.CreateConnection(_settings.SalaryConnection);

            var response = await conn.QueryFirstAsync<Guid>(
                "AddSalary",
                parameter,
                commandType: CommandType.StoredProcedure
            );
            return response;


        }
    }
}
