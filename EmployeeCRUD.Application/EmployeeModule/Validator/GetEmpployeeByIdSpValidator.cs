using EmployeeCRUD.Application.EmployeeModule.Queries;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Validator
{
    public class GetEmpployeeByIdSpValidator:AbstractValidator<GetEmployeeByIdSpQuery>
    {
        private readonly AppDbContext dbContext;
        
        public GetEmpployeeByIdSpValidator(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Employee ID is required.")
                .MustAsync(async (id, cancellation) =>
                {
                    return await dbContext.Employees.AnyAsync(e => e.Id == id, cancellation);
                }).WithMessage("Employee with the specified ID does not exist.");
        }


    }
   
}
