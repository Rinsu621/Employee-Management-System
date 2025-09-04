using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Interfaces;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Employees
{
  public record DeleteEmployeeCommand(Guid Id):IRequest<DeleteEmployeeResponse>;
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, DeleteEmployeeResponse>
    {
       
        private readonly AppDbContext dbContext;

        public DeleteEmployeeHandler( AppDbContext _dbContext)
        {
           
            dbContext = _dbContext;
        }
       
        public async Task<DeleteEmployeeResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var existingEmployee = await dbContext.Employees.FindAsync(request.Id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.Id}' not found.");
            }
            dbContext.Employees.Remove(existingEmployee);
            await dbContext.SaveChangesAsync();

            return new DeleteEmployeeResponse
            {
                Success = true,
                Message = "Employee removed successfully."
            };
        }

      
    }
}
