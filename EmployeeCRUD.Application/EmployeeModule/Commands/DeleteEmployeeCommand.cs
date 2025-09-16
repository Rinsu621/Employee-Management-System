using Ardalis.GuardClauses;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
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
       
        private readonly IAppDbContext dbContext;

        public DeleteEmployeeHandler(IAppDbContext _dbContext)
        {
           
            dbContext = _dbContext;
        }
       
        public async Task<DeleteEmployeeResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var existingEmployee = await dbContext.Employees.FindAsync(request.Id);
            Guard.Against.Null(existingEmployee, nameof(existingEmployee), $"Employee with Id '{request.Id}' not found.");
            dbContext.Employees.Remove(existingEmployee);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteEmployeeResponse
            {
                Success = true,
                Message = "Employee removed successfully."
            };
        }

      
    }
}
