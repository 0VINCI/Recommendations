using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Recommendations.Shared.Infrastructure.Commands;
using Recommendations.Shared.Infrastructure.Cors;
using Recommendations.Shared.Infrastructure.Events;
using Recommendations.Shared.Infrastructure.Options;
using Recommendations.Shared.Infrastructure.Queries;
using Recommendations.Shared.Infrastructure.Services;
using Recommendations.Shared.Infrastructure.UserContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Recommendations.Shared.Infrastructure.Exceptions;

namespace Recommendations.Shared.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddSharedFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOpts = configuration
                    .GetSection(JwtOptions.SectionName)
                    .Get<JwtOptions>();

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOpts.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOpts.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOpts.Key))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue("jwt-token", out var cookieToken))
                        {
                            context.Token = cookieToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("MustBeAdmin", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Role, "Admin");
            });
        });


        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        services.AddCommands(assemblies);
        services.AddQueries(assemblies);
        services.AddEvents(assemblies);
        services.AddEndpointsApiExplorer();
        services.AddOptions(configuration);
        services.AddSharedCors(configuration);
        services.AddServices();
        services.AddUserContext();
        services.AddCustomSwagger();
        services.AddCustomException();
        
        return services;
    }

    public static WebApplication UseSharedFramework(this WebApplication app)
    {
        app.UseSharedCors();
        app.UseAuthentication();
        app.UseAuthorization();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        return app;
    }
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, string title = "Recommendation API", string version = "v1")
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });
        });

        return services;
    }

    public static WebApplication UseCustomSwagger(this WebApplication app, string version = "v1")
    { 
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"API {version}");
            c.RoutePrefix = string.Empty;
        });

        return app;
    }
}
