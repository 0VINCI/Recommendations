using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.ModuleDefinition;
using Recommendations.Tracking.Core;

namespace Recommendations.Tracking.Api;

public class TrackingModule : ModuleDefinition
{
    public override string ModulePrefix => "/tracking";
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
    }
}