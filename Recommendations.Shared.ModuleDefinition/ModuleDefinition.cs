using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Recommendations.Shared.ModuleDefinition;

public abstract class ModuleDefinition
{
    internal string ModuleName => GetType().Name;
    public abstract string ModulePrefix { get; }
    public abstract void AddDependencies(IServiceCollection services, IConfiguration configuration);
    public abstract void CreateEndpoints(IEndpointRouteBuilder routeBuilder);
}