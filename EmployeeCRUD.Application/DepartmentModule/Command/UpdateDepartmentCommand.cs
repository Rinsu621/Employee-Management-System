using Ardalis.GuardClauses;
using EmployeeCRUD.Application.Department.Dtos;
using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Department.Command
{
    public record UpdateDepartmentCommand(Guid DepartmentId, DepartmentCreateDto department): IRequest<DepartmentResultDto>;

    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, DepartmentResultDto>
    {
        private readonly IAppDbContext dbContext;

        public UpdateDepartmentHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<DepartmentResultDto> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department= await dbContext.Departments.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == request.DepartmentId, cancellationToken);

           Guard.Against.Null(department, nameof(department), $"Department with Id '{request.DepartmentId}' not found.");

          
            department.DeptName = request.department.DeptName;

            await dbContext.SaveChangesAsync(cancellationToken);

            return new DepartmentResultDto
            {
                Id = department.Id,
                Name = department.DeptName,
                Employees = department.Employees?.Select(e => new EmployeeReturnForDepartmentDto
                {
                    Id = e.Id,
                    Name = e.EmpName,
                    Email = e.Email,
                    Phone = e.Phone
                }).ToList() ?? new List<EmployeeReturnForDepartmentDto>()
            };
        }
    }

}
