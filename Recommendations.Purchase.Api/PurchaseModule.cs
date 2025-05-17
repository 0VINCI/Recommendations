using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Purchase.Core;
using Recommendations.Shared.ModuleDefinition;

namespace Recommendations.Purchase.Api;

internal sealed class PurchaseModule : ModuleDefinition
{
    public override string ModulePrefix => "/purchase";

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
    }
}