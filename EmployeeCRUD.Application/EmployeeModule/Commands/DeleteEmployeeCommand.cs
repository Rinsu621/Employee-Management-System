using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
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
