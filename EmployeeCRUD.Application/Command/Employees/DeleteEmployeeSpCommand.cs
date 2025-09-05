using EmployeeCRUD.Application.Dtos.Departments;
using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Employees
{
    public record DeleteEmployeeSpCommand(Guid EmpId):IRequest<DeleteEmployeeResponse>;

    public class DeleteEmployeeSpHandler:IRequestHandler<DeleteEmployeeSpCommand, DeleteEmployeeResponse>
    {
        private readonly AppDbContext dbContext;

        public DeleteEmployeeSpHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DeleteEmployeeResponse> Handle(DeleteEmployeeSpCommand request, CancellationToken cancellationToken)
        {
            //not interpolated as treated literally
            //var rowAffected = await dbContext.Database.ExecuteSqlRawAsync("EXEC DeleteEmployee @Id={request.EmpId}");
            var rowAffected = await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteEmployee @Id = {request.EmpId}");

            if (rowAffected == 0)
            {
                throw new KeyNotFoundException($"Department with Id '{request.EmpId}' not found.");
            }

            return new DeleteEmployeeResponse
            {
                Success = true,
                Message = "Department removed successfully."
            };
        }
    }
}
