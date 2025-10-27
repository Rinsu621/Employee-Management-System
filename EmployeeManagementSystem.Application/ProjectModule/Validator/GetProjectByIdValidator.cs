using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Application.ProjectModule.Query;
using EmployeeManagementSystem.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.ProjectModule.Validator
{
  
    public class GetProjectByIdValidator : AbstractValidator<GetProjectByIdQuery>
    {
        private readonly IAppDbContext dbContext;
        public GetProjectByIdValidator(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Project ID is required.")
                .MustAsync(async (Id, cancellation) =>
                {
                    return await dbContext.Projects.AnyAsync(e => e.Id == Id, cancellation);
                }).WithMessage("Project with the specified ID does not exist.");
        }
    }
}
