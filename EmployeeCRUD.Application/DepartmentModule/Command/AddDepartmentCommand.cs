using EmployeeCRUD.Application.Department.Dtos;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Application.Exceptions;

using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.DepartmentModule.Command
{
    public record AddDepartmentCommand(DepartmentCreateDto department):IRequest<DepartmentResultDto>;

    public class  AddDepartmentHandler:IRequestHandler<AddDepartmentCommand , DepartmentResultDto>
    {
        
           private readonly AppDbContext dbContext;
            public AddDepartmentHandler(AppDbContext _dbContext, IValidator<DepartmentCreateDto> _validator)
             {
                dbContext = _dbContext;
             }
        public async Task<DepartmentResultDto> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
           
            var entity = new Domain.Entities.Department
            {
                DeptName = request.department.DeptName
            };

            dbContext.Departments.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DepartmentResultDto
            {
                Id = entity.Id,
                Name = entity.DeptName,
                Employees = entity.Employees.Select(e => new EmployeeReturnForDepartmentDto
                     {
                          Id = e.Id,
                         Name = e.EmpName,
                         Email = e.Email,
                         Phone= e.Phone
                     }).ToList()
            };



        }

    }

}
