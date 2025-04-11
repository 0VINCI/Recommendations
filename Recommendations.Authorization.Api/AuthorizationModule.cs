using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Authorization.Application.Commands;
using Recommendations.Authorization.Core;
using Recommendations.Authorization.Core.Queries;
using Recommendations.Authorization.Infrastructure;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.ModuleDefinition;

namespace Recommendations.Authorization.Api;

internal sealed class AuthorizationModule : ModuleDefinition
{
    public override string ModulePrefix => "/authorization";    

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore(configuration);
        services.AddInfrastructure(configuration);
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/getUsers", async (                 
                [FromServices] IQueryDispatcher queryDispatcher, CancellationToken cancellationToken = default) 
            => await queryDispatcher.QueryAsync(new GetUsers(), cancellationToken));

        app.MapPost("/signIn", async (
            [FromBody] SignIn command,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapPost("/signUp", async (
            [FromBody] SignUp command,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapPost("/changePassword", async (
            [FromBody] ChangePassword command,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapPost("/remindPassword", async (
            [FromBody] RemindPassword command,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command);
            return Results.StatusCode(StatusCodes.Status200OK);
        });
    }
}