namespace Recommendations.Shared.Abstractions.Commands.CommandWithResult;

public interface ICommandDispatcherWithResult
{
    Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommandWithResult<TResult>;
}
