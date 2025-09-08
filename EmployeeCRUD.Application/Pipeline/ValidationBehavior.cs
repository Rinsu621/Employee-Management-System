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
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IServiceProvider _serviceProvider;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, IServiceProvider serviceProvider)
        {
            _validators = validators;
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var failures = new List<FluentValidation.Results.ValidationFailure>();
            Console.WriteLine("Starting validation...");

    
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                failures.AddRange(results.SelectMany(r => r.Errors).Where(f => f != null));
            }

            // Validating the id of employee 
            var idProperty = request.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                var idValue = (Guid)idProperty.GetValue(request);
                if (idValue != Guid.Empty)
                {
                    Console.WriteLine($"Validating Id: {idValue}");
                    var validator = _serviceProvider.GetService<IValidator<Guid>>();
                    if (validator != null)
                    {
                        var context = new ValidationContext<Guid>(idValue);
                        var validationResults = await validator.ValidateAsync(context, cancellationToken);
                        failures.AddRange(validationResults.Errors);
                    }
                    else
                    {
                        Console.WriteLine("EntityIdValidator not found for Guid.");
                    }
                }
            }

            var props = request.GetType().GetProperties()
                .Where(p => p.PropertyType != typeof(string) && !p.PropertyType.IsPrimitive);

            foreach (var prop in props)
            {
                var value = prop.GetValue(request);
                if (value == null) continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(prop.PropertyType);
                var validator = _serviceProvider.GetService(validatorType) as IValidator;
                if (validator != null)
                {
                    var context = new ValidationContext<object>(value);
                    var result = await validator.ValidateAsync(context, cancellationToken);
                    failures.AddRange(result.Errors.Where(f => f != null));
                }
            }

            if (failures.Count > 0)
            {
                var errors = failures
                    .GroupBy(f => f.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(f => f.ErrorMessage).ToArray()
                    );
                Console.WriteLine("Validation failed:");
                throw new CustomValidationException(errors);
            }

            return await next();
        }
    }

}

