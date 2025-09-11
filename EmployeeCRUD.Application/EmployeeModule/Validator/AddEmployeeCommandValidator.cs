
using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Application.Interface;
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
    public class AddEmployeeCommandValidator:AbstractValidator<AddEmployeeCommand>

    {
        private readonly IAppDbContext dbContext;
        public AddEmployeeCommandValidator(EmployeeDtoValidator employeeValidator)
        {
            

            RuleFor(x => x.employee)
            .NotNull().WithMessage("Employee data is required.")
            .SetValidator(employeeValidator);

        }

    }
}
