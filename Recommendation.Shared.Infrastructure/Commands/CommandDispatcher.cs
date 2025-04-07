using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendation.Shared.Infrastructure.Commands;

internal sealed class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
    {
        public async Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            using var scope = serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
            await handler.HandleAsync(command);
        }
    }
