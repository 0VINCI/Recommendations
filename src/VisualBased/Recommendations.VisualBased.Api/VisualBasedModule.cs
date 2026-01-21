using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.ModuleDefinition;
using Recommendations.VisualBased.Core;

namespace Recommendations.VisualBased.Api;

public class VisualBasedModule : ModuleDefinition
{
    public override string ModulePrefix => "/visual";
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
    }
}