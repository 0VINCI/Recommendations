using Recommendations.Middlewares;
using Recommendations.Shared.Infrastructure;
using Recommendations.Shared.ModuleDefinition;

namespace Recommendations;

public static class Extensions
{
    public static void AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSharedFramework(configuration);
        services.AddHttpContextAccessor();
        services.AddModules(configuration);
    }

    public static WebApplication UseApiDependencies(this WebApplication app)
    {
        app.UseSharedFramework();
        app.UseModulesEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.UseCustomSwagger();
        }

        return app;
    }
}