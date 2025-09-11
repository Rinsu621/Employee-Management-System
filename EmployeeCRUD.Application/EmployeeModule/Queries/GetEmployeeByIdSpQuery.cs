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
    public record GetEmployeeByIdSpQuery([property: FromRoute] Guid Id) : IRequest<EmployeeResponseKeyless>;

    public class GetEmployeeByIdSpHandler : IRequestHandler<GetEmployeeByIdSpQuery, EmployeeResponseKeyless>
    {
        private readonly AppDbContext dbContext;
        public GetEmployeeByIdSpHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<EmployeeResponseKeyless> Handle(GetEmployeeByIdSpQuery request, CancellationToken cancellationToken)
        {
            var employee = dbContext.Set<EmployeeResponseKeyless>()
        .FromSqlRaw("EXEC GetEmployeeById @Id = {0}", request.Id) 
        .AsNoTracking() 
        .AsEnumerable() 
        .FirstOrDefault(); 

           
            return employee;
        }

    }
}
