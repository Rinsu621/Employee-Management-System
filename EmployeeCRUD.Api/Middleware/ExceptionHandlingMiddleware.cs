using EmployeeCRUD.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EmployeeCRUD.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
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
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                var (statusCode, userMessage) = ex switch
                {
                    CustomValidationException cvEx => (StatusCodes.Status400BadRequest, "Validation failed. Please correct the input."),
                    ArgumentException => (StatusCodes.Status400BadRequest, "Invalid argument provided."),
                    InvalidOperationException => (StatusCodes.Status400BadRequest, "Operation cannot be performed."),
                    KeyNotFoundException => (StatusCodes.Status404NotFound, "Requested resource not found."),
                    UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "You are not authorized to perform this action."),
                    AlreadyExistsException => (StatusCodes.Status409Conflict, "Resource already exists."),
                    DbUpdateException dbEx when dbEx.InnerException is SqlException sqlEx && sqlEx.Number == 2627 => (StatusCodes.Status409Conflict, "Duplicate entry detected."),
                    _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.")
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = userMessage,
                    details = ex is CustomValidationException cvExDetails ? cvExDetails.Errors : null,
                    statusCode
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
