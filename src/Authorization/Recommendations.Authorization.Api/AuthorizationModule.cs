using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Authorization.Application;
using Recommendations.Authorization.Core;
using Recommendations.Authorization.Infrastructure;
using Recommendations.Authorization.Shared.Commands;
using Recommendations.Authorization.Shared.DTO;
using Recommendations.Authorization.Shared.Queries;
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
        services.AddApplication();
        services.AddInfrastructure();
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/getAllUsers", async (                 
                [FromServices] IQueryDispatcher queryDispatcher, CancellationToken cancellationToken = default) 
            => await queryDispatcher.QueryAsync(new GetAllUsers(), cancellationToken)
        ).RequireAuthorization();

        app.MapPost("/signIn", async (
            [FromBody] SignInDto signInDto,
            [FromServices] IQueryDispatcher queryDispatcher,
            HttpContext httpContext,
            CancellationToken cancellationToken = default) =>
        {
            var signedInDto = await queryDispatcher.QueryAsync(new SignIn(signInDto), cancellationToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(60),
            };

            httpContext.Response.Cookies.Append("jwt-token", signedInDto.Token, cookieOptions);

            return Results.Ok(new
            {
                signedInDto.IdUser,
                signedInDto.Name,
                signedInDto.Surname,
                signedInDto.Email,
            });
        });
        
        app.MapPost("/signOut", (HttpContext httpContext) =>
        {
            httpContext.Response.Cookies.Delete("jwt-token");
            return Results.Ok();
        });

        app.MapPost("/signUp", async (
            [FromBody] SignUpDto dto,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(new SignUp(dto),cancellationToken);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapPost("/changePassword", async (
            [FromBody] ChangePasswordDto dto,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(new ChangePassword(dto), cancellationToken);
            return Results.StatusCode(StatusCodes.Status200OK);
        });

        app.MapPost("/remindPassword", async (
            [FromBody] RemindPassword command,
            [FromServices] ICommandDispatcher commandDispatcher, CancellationToken cancellationToken = default) =>
        {
            await commandDispatcher.SendAsync(command, cancellationToken);
            return Results.StatusCode(StatusCodes.Status200OK);
        });
    }
}