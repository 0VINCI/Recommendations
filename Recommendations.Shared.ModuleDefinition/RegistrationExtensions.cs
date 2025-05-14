using System.Collections.Concurrent;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Recommendations.Shared.ModuleDefinition;

public static class RegistrationModules
{
    private static readonly ConcurrentDictionary<string, ModuleDefinition> RegisteredModules = new();

    public static void RegisterModule<TModule>(Func<TModule> moduleFactory = default) where TModule : ModuleDefinition
    {
        var moduleDefinition = moduleFactory is not null
            ? moduleFactory()
            : Activator.CreateInstance<TModule>();

        RegisteredModules.TryAdd(moduleDefinition.ModuleName, moduleDefinition);
    }

    public static void AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        foreach (var module in RegisteredModules.Values)
        {
            module.AddDependencies(services, configuration);
        }
    }

    public static WebApplication UseModulesEndpoints(this WebApplication app)
    {
        foreach (var module in RegisteredModules.Values)
        {
            var group = app.MapGroup(module.ModulePrefix);

            if (module.RequireAuthorization)
                group.RequireAuthorization();

            module.CreateEndpoints(group);
        }

        return app;
    }
}