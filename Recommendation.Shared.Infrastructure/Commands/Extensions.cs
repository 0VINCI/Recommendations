﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendation.Shared.Infrastructure.Commands
{
    internal static class Extensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.Scan(s => s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            return services;
        }
    }
}