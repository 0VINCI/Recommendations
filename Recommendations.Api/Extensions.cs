using Recommendation.Shared.Infrastructure;
using Recommendations.Shared;

namespace recommendations;

public static class Extensions
{
    public static void AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModules(configuration);
        services.AddHttpContextAccessor();
        services.AddSharedFramework(configuration);
        services.AddCustomSwagger();
        services.AddAuthorization();
    }

    public static WebApplication UseApiDependencies(this WebApplication app)
    {
        app.UseModulesEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.UseCustomSwagger();
        }

        return app;
    }
}