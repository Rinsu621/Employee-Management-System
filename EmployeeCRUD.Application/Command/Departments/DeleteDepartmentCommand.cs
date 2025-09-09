using EmployeeCRUD.Application.Dtos.Departments;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Command.Departments
{
    public record DeleteDepartmentCommand([property:FromRoute]Guid Id) : IRequest<DeleteDepartmentResponse>;

    public class DeleteDepartmentHandler: IRequestHandler<DeleteDepartmentCommand, DeleteDepartmentResponse>
    {
        private readonly AppDbContext dbContext;
        public DeleteDepartmentHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<DeleteDepartmentResponse> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var existingDepartment = await dbContext.Departments.FindAsync(request.Id);
            if (existingDepartment == null)
            {
                throw new KeyNotFoundException($"Department with Id '{request.Id}' not found.");
            }
            dbContext.Departments.Remove(existingDepartment);
            await dbContext.SaveChangesAsync();
            return new DeleteDepartmentResponse
            {
                Success = true,
                Message = "Department removed successfully."
            };
        }
    }
}
