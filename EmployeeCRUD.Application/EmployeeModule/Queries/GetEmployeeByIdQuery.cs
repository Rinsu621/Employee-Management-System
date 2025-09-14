using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Domain.Interface;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Queries
{
   public record GetEmployeeByIdQuery(Guid Id) : IRequest<EmployeeResponseDto>;

    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeResponseDto>
    {
        private readonly Domain.Interface.IAppDbContext dbContext;
        public GetEmployeeByIdHandler(Domain.Interface.IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<EmployeeResponseDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await dbContext.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            return new EmployeeResponseDto
            {
                Id = employee.Id,
                EmpName = employee.EmpName,
                Email = employee.Email,
                Phone = employee.Phone,
                
                CreatedAt = employee.CreatedAt
            };
        }
    }
}