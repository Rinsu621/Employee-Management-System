using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Queries
{
    public record GetAllEmployeesQuery() : IRequest<IEnumerable<EmployeeResponseDto>>;
    

    public class GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeResponseDto>>
    {
        private readonly AppDbContext dbContext;
        public GetAllEmployeesHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<IEnumerable<EmployeeResponseDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await dbContext.Employees.Include(e=>e.Department).ToListAsync(cancellationToken);
            return employees.Select(e => new EmployeeResponseDto
            {
                Id = e.Id,
                Name = e.EmpName,
                Email = e.Email,
                Phone = e.Phone,
                DepartmentName = e.Department != null ? e.Department.DeptName : null,
                CreatedAt = e.CreatedAt
            });

        }
    }
}
