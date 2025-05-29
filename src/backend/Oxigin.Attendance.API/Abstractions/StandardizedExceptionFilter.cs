using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Oxigin.Attendance.Shared.Models.Responses;

namespace Oxigin.Attendance.API.Abstractions;

/// <summary>
/// Global exception filter to return standardized error responses for unhandled exceptions.
/// This filter ensures that all unhandled exceptions in API controllers are caught and returned
/// as a consistent <see cref="StandardizedError"/> object, improving error handling and client experience.
/// </summary>
public class StandardizedExceptionFilter : IExceptionFilter
{
    /// <summary>
    /// Called when an unhandled exception occurs in the MVC pipeline.
    /// Converts the exception into a standardized error response and sets it as the result.
    /// </summary>
    /// <param name="context">The exception context containing exception details and HTTP context.</param>
    public void OnException(ExceptionContext context)
    {
        // Get the thrown exception.
        var ex = context.Exception;

        // Build a standardized error object with origin, message, and stack trace.
        var error = new StandardizedError
        {
            // The class or type where the exception originated, or "Unknown" if not available.
            Origin = ex.TargetSite?.DeclaringType?.Name ?? "Unknown",
            // The exception message for client diagnostics.
            Message = ex.Message,
            // Additional data, such as the stack trace, for debugging.
            Data = new Dictionary<string, object>
            {
                { nameof(ex.StackTrace), ex.StackTrace ?? string.Empty }
            }
        };

        // Set the HTTP response to 400 Bad Request with the standardized error payload.
        context.Result = new BadRequestObjectResult(error);
        // Mark the exception as handled so it does not propagate further.
        context.ExceptionHandled = true;
    }
}
