using Microsoft.AspNetCore.Diagnostics;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.WebApi.Models.Commons;

namespace SpeakingShorts.WebApi.Middlewares;

public class ForbiddenExceptionMiddleware : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ForbiddenException ex)
            return false;

        httpContext.Response.StatusCode = ex.StatusCode;

        await httpContext.Response.WriteAsJsonAsync(new Response
        {
            StatusCode = ex.StatusCode,
            Message = ex.Message,
        });

        return true;
    }
}
