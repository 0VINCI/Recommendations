using Recommendations.Authorization.Application.Exceptions;
using Recommendations.Authorization.Infrastructure.DAL.Repositories;
using Recommendations.Authorization.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Authorization.Application.Commands.Handlers;

internal sealed class SignInHandler(IUserRepository userRepository) : ICommandHandler<SignIn>
{
    public async Task HandleAsync(SignIn command, CancellationToken cancellationToken = default)
    {
        var data = command.SignInDto;

        var user = await userRepository.GetUser(data.Email);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        if (data.Password != user.Password)
        {
            throw new InvalidPasswordException();
        }
    } 
}