using EmployeeCRUD.Application.ProjectModule.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Validator
{
    public class AddProjectCommandValidator: AbstractValidator<AddProjectCommand>

    {
        public AddProjectCommandValidator() 
        {
            RuleFor(p=>p.ProjectName)
                  .NotEmpty().WithMessage("Project name is required.")
                .MaximumLength(100).WithMessage("Project name must not exceed 100 characters.");

            RuleFor(p => p.Description)
               .NotEmpty().WithMessage("Project description is required.")
               .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(p => p.StartDate)
               .NotEmpty().WithMessage("Start date is required.")
               .LessThanOrEqualTo(p => p.EndDate ?? DateTime.MaxValue)
               .WithMessage("Start date must be before or equal to end date.");

            RuleFor(p => p.EndDate)
               .GreaterThanOrEqualTo(p => p.StartDate)
               .When(p => p.EndDate.HasValue)
               .WithMessage("End date must be after start date.");

            RuleFor(p => p.Budget)
               .GreaterThanOrEqualTo(0).WithMessage("Budget must be a positive number.");


        }
    }
}
