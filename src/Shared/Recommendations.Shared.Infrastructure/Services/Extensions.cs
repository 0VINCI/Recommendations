using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.Abstractions.Email;
using Recommendations.Shared.Abstractions.Services;

namespace Recommendations.Shared.Infrastructure.Services;

public static class Extensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpClient<ISendEmailService, SendEmailService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        
        return services;
    }
}