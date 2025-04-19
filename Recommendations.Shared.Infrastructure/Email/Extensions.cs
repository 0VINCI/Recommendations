using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.Abstractions.Email;

namespace Recommendations.Shared.Infrastructure.Email;

public static class Extensions
{
    public static IServiceCollection AddEmail(this IServiceCollection services)
    {
        services.AddHttpClient<ISendEmailService, SendEmailService>();
        
        return services;
    }
}