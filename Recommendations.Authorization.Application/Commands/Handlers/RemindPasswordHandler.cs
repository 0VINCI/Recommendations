using Recommendations.Authorization.Application.Exceptions;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Authorization.Application.Commands.Handlers;

internal sealed class RemindPasswordHandler : ICommandHandler<RemindPassword>
{
    public Task HandleAsync(RemindPassword command, CancellationToken cancellationToken = default)
    {
        throw new PasswordNotTheSameException();
    }
}
