namespace Recommendations.Shared.Abstractions.Commands.CommandWithResult;

public interface ICommandHandlerWithResult<TCommand, TResult>
    where TCommand : ICommandWithResult<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
