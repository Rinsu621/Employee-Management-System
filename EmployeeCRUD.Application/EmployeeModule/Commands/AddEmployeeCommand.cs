using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record AddEmployeeCommand(EmployeeDto employee) : IRequest<EmployeeResponseDto>;

    public class AddEmployeeHandler : IRequestHandler<AddEmployeeCommand, EmployeeResponseDto>
    {
        private readonly AppDbContext dbContext;

        public AddEmployeeHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<EmployeeResponseDto> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            
            var entity = new  Employee
            {
                EmpName = request.employee.EmpName,
                Email = request.employee.Email,
                Phone = request.employee.Phone
            };

            // Add to the database
            dbContext.Employees.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a response DTO
            return new EmployeeResponseDto
            {
                Id = entity.Id,
                Name = entity.EmpName,
                Email = entity.Email,
                Phone = entity.Phone,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
