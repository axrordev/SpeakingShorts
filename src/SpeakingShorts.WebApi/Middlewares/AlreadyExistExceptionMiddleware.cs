
using Microsoft.AspNetCore.Diagnostics;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.WebApi.Models.Commons;

namespace Axidel.WebApi.Middlewares;

public class AlreadyExistExceptionMiddleware : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not AlreadyExistException ex)
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
