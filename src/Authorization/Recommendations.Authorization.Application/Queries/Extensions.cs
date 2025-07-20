using Microsoft.Extensions.DependencyInjection;
using Recommendations.Authorization.Application.Queries.Handlers;
using Recommendations.Authorization.Shared.DTO;
using Recommendations.Authorization.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Authorization.Application.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetAllUsers, IReadOnlyCollection<UserDto>>, GetAllUsersHandler>();
        services.AddScoped<IQueryHandler<SignIn, SignedInDto>, SignInHandler>();
        services.AddScoped<IQueryHandler<GetUserById, UserInfoDto>, GetUserBydHandler>();

        return services;
    }
}