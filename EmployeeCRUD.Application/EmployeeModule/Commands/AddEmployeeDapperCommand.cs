using Ardalis.GuardClauses;
using Dapper;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record AddEmployeeDapperCommand(string EmpName, string Email, string Phone, string Role): IRequest<EmployeeResponseDto>;

    public class AddEmployeeDapperHandler : IRequestHandler<AddEmployeeDapperCommand, EmployeeResponseDto>
    {
        private readonly IDbConnection connection;
        private readonly IConfiguration configuration;
        private readonly IEmailService emailService;
        public AddEmployeeDapperHandler(IDbConnection _connection, IConfiguration _configuration, IEmailService _emailService)
        {
            connection = _connection;
            configuration = _configuration;
            emailService = _emailService;
        }
        public async Task<EmployeeResponseDto> Handle(AddEmployeeDapperCommand request, CancellationToken cancellationToken)
        {
            var defaultPassword = configuration["DefaultPassword:Password"];
            var parameters = new
            {
                EmpName = request.EmpName,
                Email = request.Email,
                Phone = request.Phone,
                RoleName = request.Role,
                DefaultPassword = defaultPassword,

            };

            var result = await connection.QuerySingleOrDefaultAsync<EmployeeResponseDto>(
                "AddEmployee",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            Guard.Against.Null(result, nameof(result), "Failed to add employee");
            BackgroundJob.Enqueue(() => emailService.SendEmployeeCredentialsAsync(request.Email, defaultPassword));
            return result;

        }
    }
}
