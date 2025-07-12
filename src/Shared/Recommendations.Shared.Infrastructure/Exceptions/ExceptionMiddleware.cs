using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Recommendations.Shared.Infrastructure.Exceptions;

internal sealed class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger) : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private const string SomethingWentWrong = "Coś poszło nie tak!";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await WriteErrorToResponse(context, exception);
        }
    }

    private async Task WriteErrorToResponse(HttpContext context, Exception exception)
    {
        if (exception is HumanPresentableException humanReadable)
        {
            context.Response.StatusCode = (int)humanReadable.ExceptionCategory.AsStatusCode();
            await context.Response.WriteAsJsonAsync(new ErrorPayload(humanReadable.Message));
            return;
        }
        else if (exception is CustomException customException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorPayload(customException.Message));
            return;
        }

        _logger.LogError(exception, exception.Message);
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new ErrorPayload(SomethingWentWrong));
    }
}
