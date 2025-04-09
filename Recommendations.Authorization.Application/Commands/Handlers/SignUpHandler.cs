using Recommendations.Authorization.Application.Exceptions;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Authorization.Application.Commands.Handlers;

internal sealed class SignUpHandler : ICommandHandler<SignUp>
{
    public Task HandleAsync(SignUp command, CancellationToken cancellationToken = default)
    {
        throw new UserAlreadyExistsException();
    }
}
