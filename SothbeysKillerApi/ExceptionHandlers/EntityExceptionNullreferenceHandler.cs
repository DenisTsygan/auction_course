

using Microsoft.AspNetCore.Diagnostics;
using SothbeysKillerApi.Exceptions;

namespace SothbeysKillerApi.ExceptionHandlers;
public class EntityExceptionNullreferenceHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is EntityExceptionNullreference ex)
        {
            httpContext.Response.StatusCode = 404;

            await httpContext.Response
                .WriteAsJsonAsync(new
                {
                    entity = ex.Entity,
                },
                cancellationToken);

            return true;
        }

        return false;
    }
}