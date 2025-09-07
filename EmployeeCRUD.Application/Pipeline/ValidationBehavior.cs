using EmployeeCRUD.Application.Exceptions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Pipeline
{


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
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                failures.AddRange(results.SelectMany(r => r.Errors).Where(f => f != null));
            }
            var props = request!.GetType().GetProperties()
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

                throw new CustomValidationException(errors);
            }

            return await next();
        }
    }

}
