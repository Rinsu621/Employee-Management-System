using EmployeeCRUD.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmployeeCRUD.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next; // represent next component in the pipeline
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); //to invoke the next middleware or endpoints
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                var statusCode = ex switch
                {
                    CustomValidationException => StatusCodes.Status400BadRequest,
                    InvalidOperationException => StatusCodes.Status400BadRequest,
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError

                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var response = ex switch
                {
                    CustomValidationException validationEx => JsonSerializer.Serialize(new
                    {
                        errors = validationEx.Errors,
                        statusCode = context.Response.StatusCode
                    }),
                    _ => JsonSerializer.Serialize(new
                    {
                        error = ex.Message,
                        statusCode = context.Response.StatusCode
                    })
                };

                await context.Response.WriteAsync(response);

            }
        }
    }
}
