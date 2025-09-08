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
    public record DeleteEmployeeSpCommand(Guid Id):IRequest<DeleteEmployeeResponse>;

    public class DeleteEmployeeSpHandler:IRequestHandler<DeleteEmployeeSpCommand, DeleteEmployeeResponse>
    {
        private readonly AppDbContext dbContext;

        public DeleteEmployeeSpHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<DeleteEmployeeResponse> Handle(DeleteEmployeeSpCommand request, CancellationToken cancellationToken)
        {
          
            var rowAffected = await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteEmployee @Id = {request.Id}");

            return new DeleteEmployeeResponse
            {
                Success = true,
                Message = "Employee removed successfully."
            };
        }
    }
}
