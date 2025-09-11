using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Data.Keyless;
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
    public record GetEmployeeByIdSpQuery([property: FromRoute] Guid Id) : IRequest<EmployeeResponseDto>;

    public class GetEmployeeByIdSpHandler : IRequestHandler<GetEmployeeByIdSpQuery, EmployeeResponseDto>
    {
        private readonly IAppDbContext dbContext;
        public GetEmployeeByIdSpHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<EmployeeResponseDto> Handle(GetEmployeeByIdSpQuery request, CancellationToken cancellationToken)
        {
            var employee = dbContext.Employees
                         .FromSqlRaw("EXEC GetEmployeeById @Id = {0}", request.Id) 
                         .AsNoTracking() 
                        .Select(x => new EmployeeResponseDto 
                        {
                            Id = x.Id,
                            Name = x.EmpName,
                            Email = x.Email,
                            Phone = x.Phone,
                            CreatedAt = x.CreatedAt
                        })
                        .FirstOrDefault(); 

           
            return employee;
        }

    }
}
