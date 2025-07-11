using System.Net;
using Recommendations.Authorization.Application.Exceptions;
using Recommendations.Authorization.Core.Exceptions;

namespace Recommendations.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UserNotFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }
        catch (InvalidPasswordException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }
        catch (CustomException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(ex.Message);
        }
        catch (Exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
