using EmployeeCRUD.Application.EmployeeModule.Queries;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRUD.Application.EmployeeModule.Validator
{
    public class GetEmpployeeByIdSpValidator:AbstractValidator<GetEmployeeByIdSpQuery>
    {
        private readonly IAppDbContext dbContext;
        
        public GetEmpployeeByIdSpValidator(IAppDbContext _dbContext)
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
