using EmployeeCRUD.Domain.Interface;
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
        private readonly Domain.Interface.IAppDbContext dbContext;
        public GetEmployeeByIdSpHandler(Domain.Interface.IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<EmployeeResponseKeyless> Handle(GetEmployeeByIdSpQuery request, CancellationToken cancellationToken)
        {
            var employee =await dbContext.EmployeeResponseKeyless
            .FromSqlRaw("EXEC GetEmployeeById @Id = {0}", request.Id) 
            .AsNoTracking() 
            .FirstOrDefaultAsync(); 
        
            return employee;
        }

    }
}
