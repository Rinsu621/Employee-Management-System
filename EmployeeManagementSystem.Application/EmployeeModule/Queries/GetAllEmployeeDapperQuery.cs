using Dapper;
using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmployeeManagementSystem.Application.Interface;

namespace EmployeeManagementSystem.Application.EmployeeModule.Queries
{
    public record GetAllEmployeeDapperQuery(
        int Page,
        int PageSize,
        string? Role = null,
        Guid? DepartmentId = null,
        DateTime? FromDate = null,
        DateTime? ToDate = null,
        string? SearchTerm = null,
        string? SortKey = "CreatedAt",
        bool SortAsc = true
    ) : IRequest<EmployeePagedResponseDto>;

    public class GetAllEmployeeDapperHandler : IRequestHandler<GetAllEmployeeDapperQuery, EmployeePagedResponseDto>
    {
        private readonly IDbConnectionService connection;

        public GetAllEmployeeDapperHandler(IDbConnectionService _connection)
        {
            connection = _connection;
        }

        public async Task<EmployeePagedResponseDto> Handle(GetAllEmployeeDapperQuery request, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Page", request.Page);
            parameters.Add("@PageSize", request.PageSize);
            parameters.Add("@Role", request.Role);
            parameters.Add("@DepartmentId", request.DepartmentId);
            parameters.Add("@FromDate", request.FromDate);
            parameters.Add("@ToDate", request.ToDate);
            parameters.Add("@SearchTerm", request.SearchTerm);
            parameters.Add("@SortKey", request.SortKey);
            parameters.Add("@SortAsc", request.SortAsc);

            using var db= connection.CreateConnection();
            using var multi = await db.QueryMultipleAsync(
             "GetAllEmployees",
             parameters,
                commandType: CommandType.StoredProcedure
                );

            var employees = multi.Read<EmployeeResponseDto>().ToList();
            var totalCount = multi.ReadFirst<int>();

            return new EmployeePagedResponseDto
            {
                Employees = employees,
                TotalCount = totalCount
            };
        }
    }
}
