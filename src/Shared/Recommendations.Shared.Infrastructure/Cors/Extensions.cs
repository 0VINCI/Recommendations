using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.Infrastructure.Options;

namespace Recommendations.Shared.Infrastructure.Cors;
public static class Extensions
{
    private const string PolicyName = "allowed-origin";
    public static IServiceCollection AddSharedCors(this IServiceCollection services, IConfiguration configuration)
    {
        var opt = configuration
            .GetSection(CorsOptions.SectionName)
            .Get<CorsOptions>();
        
        services.AddCors(o =>
        {
            o.AddPolicy(name: PolicyName,
                builder =>
                {
                    builder
                        .WithMethods(opt.Methods)
                        .WithHeaders(opt.Headers)
                        .WithOrigins(opt.Urls)
                        .AllowCredentials();
                });
        });
        
        return services;
    }
    public static WebApplication UseSharedCors(this WebApplication app)
    {
        app.UseCors(PolicyName);
        return app;
    }
}
