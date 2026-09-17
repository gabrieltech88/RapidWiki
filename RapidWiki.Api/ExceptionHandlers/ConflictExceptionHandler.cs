using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.Exceptions;

namespace RapidWiki.Api.ExceptionHandlers;
public class ConflictExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ConflictException conflictException)
            return false;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Conflito",
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}