using Microsoft.AspNetCore.Diagnostics;

namespace SothbeysKillerApi.ExceptionHandlers;

public class ServerExceptionsHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = 500;

        await httpContext.Response
            .WriteAsJsonAsync(new
            {
                message = "Houston, we have a problem..."
            },
                cancellationToken);

        return true;
    }
}