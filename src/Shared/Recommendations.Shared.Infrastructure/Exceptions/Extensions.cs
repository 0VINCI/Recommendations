using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Recommendations.Shared.Infrastructure.Exceptions;

public static class Extensions
{
    public static IServiceCollection AddCustomException(this IServiceCollection services)
    {
        services.AddTransient<ExceptionMiddleware>();

        return services;
    }

    public static WebApplication UseExceptions(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}