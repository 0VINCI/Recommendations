using Microsoft.Extensions.DependencyInjection;
using Recommendations.Authorization.Application.Queries.Handlers;
using Recommendations.Authorization.Core.Types;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Authorization.Application.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetAllUsers, IReadOnlyCollection<User>>, GetAllUsersHandler>();
        
        return services;
    }
}