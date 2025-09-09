using EmployeeCRUD.Application.Exceptions;
using EmployeeCRUD.Application.Validator;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Pipeline
{

    //runs before actual handlder for your request
    //example in airport passenger request and security validate and then only passenger are allowed to board
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validator;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> _validator)
        {
            validator = _validator;
        }

        public async Task<TResponse>Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validator.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var failure = (await Task.WhenAll(
                    validator.Select(v => v.ValidateAsync(context, cancellationToken))
                    )).SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();
                if (failure.Count != 0)
                {
                    var errors = failure
                        .GroupBy(f => f.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(f => f.ErrorMessage).ToArray()
                        );

                    throw new CustomValidationException(errors);
                }
            }
            return await next();
        }
    }

}

