using EmployeeManagementSystem.Application.Department.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Application.Department.Dtos;

namespace EmployeeManagementSystem.Application.DepartmentModule.Command
{
    public record AddDepartmentCommand(string DeptName) :IRequest<DepartmentResultDto>;

    public class  AddDepartmentHandler:IRequestHandler<AddDepartmentCommand , DepartmentResultDto>
    {
        
           private readonly IAppDbContext dbContext;
            public AddDepartmentHandler(IAppDbContext _dbContext, IValidator<DepartmentCreateDto> _validator)
             {
                dbContext = _dbContext;
             }
        public async Task<DepartmentResultDto> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
           
            var entity = new Domain.Entities.Department
            {
                DeptName = request.DeptName
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
