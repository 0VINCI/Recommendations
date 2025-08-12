using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.ContentBased.Core;
using Recommendations.Shared.ModuleDefinition;

namespace Recommendations.ContentBased.Api;

internal sealed class ContentBasedModule : ModuleDefinition
{
    public override string ModulePrefix => "/content-based";
    public override bool RequireAuthorization => true;
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
    }
}