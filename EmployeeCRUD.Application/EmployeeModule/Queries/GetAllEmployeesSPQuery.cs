using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Data.Keyless;
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
    public record GetAllEmployeesSPQuery() : IRequest<IEnumerable<EmployeeResponseDto>>;


    public class GetAllEmployeesSPHandler : IRequestHandler<GetAllEmployeesSPQuery, IEnumerable<EmployeeResponseDto>>
    {
        private readonly IAppDbContext dbContext;
        public GetAllEmployeesSPHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<IEnumerable<EmployeeResponseDto>> Handle(GetAllEmployeesSPQuery request, CancellationToken cancellationToken)
        {

            var employees = (await dbContext.Employees
            .FromSqlRaw("EXEC GetAllEmployees")
            .AsNoTracking()
             .ToListAsync(cancellationToken))
            .Select(x => new EmployeeResponseDto
            {
                Id = x.Id,
                 Name = x.EmpName,
                 Email = x.Email,
                 Phone = x.Phone,
                 CreatedAt = x.CreatedAt
             });
            return employees;

        }
    }
}
