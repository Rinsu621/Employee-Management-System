using EmployeeCRUD.Application.Dtos.Departments;
using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Domain.Entities;
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

namespace EmployeeCRUD.Application.Command.Departments
{
    public record AddDepartmentCommand(DepartmentCreateDto department):IRequest<DepartmentResultDto>;

    public class  AddDepartmentHandler:IRequestHandler<AddDepartmentCommand , DepartmentResultDto>
    {
        
           private readonly AppDbContext dbContext;
            private readonly IValidator<DepartmentCreateDto> validator;
            public AddDepartmentHandler(AppDbContext _dbContext, IValidator<DepartmentCreateDto> _validator)
             {
                dbContext = _dbContext;
                validator = _validator;
             }
        public async Task<DepartmentResultDto> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.department, cancellationToken);

            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                throw new CustomValidationException(errors);
            }

            var exists=await dbContext.Departments.AnyAsync(d => d.DeptName.ToLower() == request.department.DeptName.ToLower(), cancellationToken);
            if (exists)
            {
                throw new AlreadyExistsException($"Department with name '{request.department.DeptName}' already exists.");
            }
            var entity = new Department
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
