using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        private readonly IAppDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public GetAllEmployeesHandler(IAppDbContext _dbContext, UserManager<ApplicationUser> _userManager)
        {
            dbContext = _dbContext;
            userManager = _userManager;
        }

        public async Task<IEnumerable<EmployeeResponseDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
           

            var employees = await dbContext.Employees
             .Include(e => e.Department)
             .OrderBy(e => e.CreatedAt)

             .ToListAsync(cancellationToken);

            var result = new List<EmployeeResponseDto>();

            foreach (var e in employees)
            {
                // Match Identity User by Email
                var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == e.Email, cancellationToken);

                var roles = new List<string>();
                if (user != null)
                {
                    roles = (await userManager.GetRolesAsync(user)).ToList();
                }

                result.Add(new EmployeeResponseDto
                {
                    Id = e.Id,
                    EmpName = e.EmpName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DepartmentName = e.Department?.DeptName,
                    Role = roles.FirstOrDefault() ?? "N/A",
                    CreatedAt = e.CreatedAt
                });
            }

            return result;
        }
    }
}


//using EmployeeCRUD.Application.EmployeeModule.Dtos;
//using EmployeeCRUD.Application.Interface;
//using EmployeeCRUD.Domain.Entities;
//using EmployeeCRUD.Infrastructure.Data;
//using FluentValidation;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EmployeeCRUD.Application.EmployeeModule.Queries
//{
//    public record GetAllEmployeesQuery(int PageNumber, int PageSize)
//      : IRequest<EmployeePagedResponseDto>;


//    public class GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesQuery, EmployeePagedResponseDto>
//    {
//        private readonly IAppDbContext dbContext;
//        private readonly UserManager<ApplicationUser> userManager;

//        public GetAllEmployeesHandler(IAppDbContext _dbContext, UserManager<ApplicationUser> _userManager)
//        {
//            dbContext = _dbContext;
//            userManager = _userManager;
//        }

//        public async Task<EmployeePagedResponseDto> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
//        {
//            var query = dbContext.Employees.Include(e => e.Department).AsQueryable();

//            var totalCount = await query.CountAsync(cancellationToken);

//            // Apply paging
//            var employees = await query
//                .Skip((request.PageNumber - 1) * request.PageSize)
//                .Take(request.PageSize)
//                .ToListAsync(cancellationToken);

//            var result = new List<EmployeeResponseDto>();

//            foreach (var e in employees)
//            {
//                var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == e.Email, cancellationToken);
//                var roles = user != null ? (await userManager.GetRolesAsync(user)).ToList() : new List<string>();

//                result.Add(new EmployeeResponseDto
//                {
//                    Id = e.Id,
//                    EmpName = e.EmpName,
//                    Email = e.Email,
//                    Phone = e.Phone,
//                    DepartmentName = e.Department?.DeptName,
//                    Role = roles.FirstOrDefault() ?? "N/A",
//                    CreatedAt = e.CreatedAt
//                });
//            }

//            return new EmployeePagedResponseDto
//            {
//                Items = result,
//                TotalCount = totalCount
//            };
//        }
//    }
//}
