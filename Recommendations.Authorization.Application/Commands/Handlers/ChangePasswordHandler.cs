using Recommendations.Authorization.Application.Exceptions;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Authorization.Application.Commands.Handlers;

internal sealed class ChangePasswordHandler : ICommandHandler<ChangePassword>
{
    public Task HandleAsync(ChangePassword command, CancellationToken cancellationToken = default)
    {
        throw new UserNotFoundException();
    }
}
