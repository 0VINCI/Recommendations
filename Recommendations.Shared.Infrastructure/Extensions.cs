using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Recommendations.Shared.Infrastructure.Commands;
using Recommendations.Shared.Infrastructure.Events;
using Recommendations.Shared.Infrastructure.Options;
using Recommendations.Shared.Infrastructure.Queries;

namespace Recommendations.Shared.Infrastructure;

public static class Extensions
{
    
    public static IServiceCollection AddSharedFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        services.AddCommands(assemblies);
        services.AddQueries(assemblies);
        services.AddEvents(assemblies);
        services.AddEndpointsApiExplorer();
        services.AddOptions(configuration);
        return services;
    }

    public static WebApplication UseSharedFramework(this WebApplication app)
    {
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        return app;
    }
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, string title = "Recommendation API", string version = "v1")
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });
        });

        return services;
    }

    public static WebApplication UseCustomSwagger(this WebApplication app, string version = "v1")
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"API {version}");
            c.RoutePrefix = string.Empty;
        });

        return app;
    }

}