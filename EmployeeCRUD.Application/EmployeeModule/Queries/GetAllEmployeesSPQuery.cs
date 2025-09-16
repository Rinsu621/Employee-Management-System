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
    public record GetAllEmployeesSPQuery() : IRequest<IEnumerable<EmployeeResponseKeyless>>;


    public class GetAllEmployeesSPHandler : IRequestHandler<GetAllEmployeesSPQuery, IEnumerable<EmployeeResponseKeyless>>
    {
        private readonly IAppDbContext dbContext;
        public GetAllEmployeesSPHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<IEnumerable<EmployeeResponseKeyless>> Handle(GetAllEmployeesSPQuery request, CancellationToken cancellationToken)
        {

            var employees = await dbContext.EmployeeResponseKeyless.FromSqlRaw($"EXECUTE GetAllEmployees").ToListAsync(cancellationToken);
            return employees;

        }
    }
}
