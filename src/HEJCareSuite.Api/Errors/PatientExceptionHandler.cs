using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HEJCareSuite.Api.Errors;

public sealed class PatientExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<PatientExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ArgumentException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid patient data.",
                Detail = exception.Message,
                Type = "https://httpstatuses.com/400"
            };

            await problemDetailsService.WriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = problem,
                    Exception = exception
                });
            return true;
        }

        logger.LogError(exception, "Unhandled API exception.");
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await problemDetailsService.WriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred.",
                    Type = "https://httpstatuses.com/500"
                },
                Exception = exception
            });
        return true;
    }
}
