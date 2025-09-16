using EmployeeCRUD.Application.EmployeeModule.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Validator
{
    public class AddEmployeeSPCommandValidator : AbstractValidator<AddEmployeeSPCommand>
    {
        public AddEmployeeSPCommandValidator(EmployeeDtoValidator employeeValidator)
        {
            RuleFor(x => x.employee)
                .NotNull().WithMessage("Employee data is required.")
                .SetValidator(employeeValidator);
        }
    }
}
