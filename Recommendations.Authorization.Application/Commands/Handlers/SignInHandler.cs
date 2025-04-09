using Recommendations.Authorization.Application.Exceptions;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Authorization.Application.Commands.Handlers;

internal sealed class SignInHandler : ICommandHandler<SignIn>
{
    public Task HandleAsync(SignIn command, CancellationToken cancellationToken = default)
    {
        throw new InvalidPasswordException();    
    }
}