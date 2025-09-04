using EmployeeCRUD.Application.Dtos.Departments;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Validator
{
    public class DepartmentDtoValidator: AbstractValidator<DepartmentCreateDto>
    {
        public DepartmentDtoValidator()
        {
            RuleFor(d => d.DeptName)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(100).WithMessage("Department name must not exceed 100 characters.");
        }

    }
}
