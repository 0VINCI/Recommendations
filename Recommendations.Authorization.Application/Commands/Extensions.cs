using Microsoft.Extensions.DependencyInjection;
using Recommendations.Authorization.Application.Commands.Handlers;
using Recommendations.Authorization.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Authorization.Application.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<SignIn>, SignInHandler>();
        services.AddScoped<ICommandHandler<SignUp>, SignUpHandler>();
        services.AddScoped<ICommandHandler<RemindPassword>, RemindPasswordHandler>();
        services.AddScoped<ICommandHandler<ChangePassword>, ChangePasswordHandler>();

        return services;
    }
}