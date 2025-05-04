using Microsoft.Extensions.DependencyInjection;

namespace Recommendations.Dictionaries.Application.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        return services;
    }
}