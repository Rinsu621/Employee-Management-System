using Dapper;
using EmployeeCRUD.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.SalaryModule.Command
{
    public record AddSalaryDapperCommand(Guid EmployeeId, decimal BasicSalary, decimal Conveyance, decimal Tax, decimal Pf, decimal ESI, string PaymentMethod, string Status, DateTime SalaryDate):IRequest<Guid>;
    
    public class AddSalaryDapperCommandHandler:IRequestHandler<AddSalaryDapperCommand, Guid>
    {
        private readonly ISalaryDbConnection salaryDbConnection;
        public AddSalaryDapperCommandHandler(ISalaryDbConnection _salaryDbConnection)
        {
            salaryDbConnection = _salaryDbConnection;
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

            var response = salaryDbConnection.CreateConnection().QueryFirstAsync<Guid>("AddSalary", parameter, commandType: CommandType.StoredProcedure);
            return await response;

        }
    }
}
