using EmployeeManagementSystem.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace EmployeeManagementSystem.Api.Middleware
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occured:{Message}", exception.Message);
            var statusCode = exception switch
            {
                CustomValidationException => StatusCodes.Status400BadRequest,
                InvalidCastException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            httpContext.Response.StatusCode = statusCode;
            object response = exception switch
            {
                CustomValidationException validationException => new
                {
                    errors = validationException.Errors,
                    statusCode = httpContext.Response.StatusCode
                },
                _ => new
                {
                    error = exception.Message,
                    statusCode = httpContext.Response.StatusCode
                }
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }

    }
}
