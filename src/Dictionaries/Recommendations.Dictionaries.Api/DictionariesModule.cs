using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Dictionaries.Application;
using Recommendations.Dictionaries.Core;
using Recommendations.Dictionaries.Infrastructure;
using Recommendations.Shared.ModuleDefinition;

namespace Recommendations.Dictionaries.Api;

internal sealed class DictionariesModule : ModuleDefinition
{
    public override string ModulePrefix => "/dic";

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
        services.AddInfrastructure();
        services.AddApplication();
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
    }
}