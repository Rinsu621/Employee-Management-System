using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Interfaces;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Queries.Employees
{
   public record GetEmployeeByIdQuery(Guid Id) : IRequest<EmployeeResponseDto>;

    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeResponseDto>
    {
        private readonly AppDbContext dbContext;
        public GetEmployeeByIdHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<EmployeeResponseDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await dbContext.Employees.FindAsync(request.Id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.Id}' not found.");
            }
            return new EmployeeResponseDto
            {
                Id = employee.Id,
                Name = employee.EmpName,
                Email = employee.Email,
                Phone = employee.Phone,
                CreatedAt = employee.CreatedAt
            };
        }
    }
}
