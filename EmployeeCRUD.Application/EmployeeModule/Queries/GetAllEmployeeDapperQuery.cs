using Dapper;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmployeeCRUD.Application.EmployeeModule.Queries
{
    public record GetAllEmployeeDapperQuery(
        int Page,
        int PageSize,
        string? Role = null,
        Guid? DepartmentId = null,
        DateTime? FromDate = null,
        DateTime? ToDate = null,
        string? SortKey = "CreatedAt",
        bool SortAsc = true
    ) : IRequest<IEnumerable<EmployeeResponseDto>>;

    public class GetAllEmployeeDapperHandler : IRequestHandler<GetAllEmployeeDapperQuery, IEnumerable<EmployeeResponseDto>>
    {
        private readonly IDbConnection connection;

        public GetAllEmployeeDapperHandler(IDbConnection _connection)
        {
            connection = _connection;
        }

        public async Task<IEnumerable<EmployeeResponseDto>> Handle(GetAllEmployeeDapperQuery request, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Page", request.Page);
            parameters.Add("@PageSize", request.PageSize);
            parameters.Add("@Role", request.Role);
            parameters.Add("@DepartmentId", request.DepartmentId);
            parameters.Add("@FromDate", request.FromDate);
            parameters.Add("@ToDate", request.ToDate);
            parameters.Add("@SortKey", request.SortKey);
            parameters.Add("@SortAsc", request.SortAsc);

            var result = await connection.QueryAsync<EmployeeResponseDto>(
                "GetAllEmployees",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}
