using Dapper;
using DocumentFormat.OpenXml.Wordprocessing;
using EmployeeManagementSystem.Application.Configuration;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Application.SalaryModule.DTO;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.SalaryModule.Queries.GetSalaryDapper
{
    public class GetSalaryDetailQueryHandler:IRequestHandler<GetSalaryDetailQuery, IEnumerable<SalaryResponseDto>>
    {
        private readonly IDbConnectionService salaryDbConnection;
        private readonly DbSettings dbSettings;
        public GetSalaryDetailQueryHandler(IDbConnectionService _salaryDbConnection, IOptions<DbSettings> options)
        {
            salaryDbConnection = _salaryDbConnection;
            dbSettings = options.Value;
        }
        public async Task<IEnumerable<SalaryResponseDto>> Handle(GetSalaryDetailQuery request, CancellationToken cancellationToken)
        {
            using var conn= salaryDbConnection.CreateConnection(dbSettings.SalaryConnection);
            var response = conn.QueryAsync<SalaryResponseDto>("GetSalaries", commandType: System.Data.CommandType.StoredProcedure);
            return await response;
        }
    }
}
