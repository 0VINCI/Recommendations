using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.Abstractions.Commands;
using Recommendations.Shared.Abstractions.Commands.CommandWithResult;

namespace Recommendations.Shared.Infrastructure.Commands;

internal sealed class CommandDispatcherWithResult(IServiceProvider serviceProvider)
    : ICommandDispatcherWithResult
{
    public async Task<TResult> SendAsync<TCommand, TResult>(
        TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommandWithResult<TResult>
    {
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandlerWithResult<TCommand, TResult>>();
        return await handler.HandleAsync(command, cancellationToken);
    }
}
