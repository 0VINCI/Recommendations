using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Authorization.Core;
using Recommendations.Shared;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Authorization;

internal sealed class AuthorizationModule : ModuleDefinition
{
    public override string ModulePrefix => "/xddd";

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
        => services.AddCore(configuration);

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/someGet", (
                [FromServices] IQueryDispatcher queryDispatcher, CancellationToken cancellationToken = default) => Task.FromResult(12));
    }
}

