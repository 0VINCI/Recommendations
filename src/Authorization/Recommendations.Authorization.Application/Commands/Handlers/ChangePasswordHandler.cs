using Recommendations.Authorization.Core.Exceptions;
using Recommendations.Authorization.Infrastructure.DAL.Repositories;
using Recommendations.Authorization.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Authorization.Application.Commands.Handlers;

internal sealed class ChangePasswordHandler(IUserRepository userRepository) : ICommandHandler<ChangePassword>
{
    public async Task HandleAsync(ChangePassword command, CancellationToken cancellationToken = default)
    {
        var data = command.ChangePasswordDto;

        var user = await userRepository.GetUser(data.Email);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        if (data.OldPassword != user.Password)
        {
            throw new InvalidPasswordException();
        }

        user.ChangePassword(data.NewPassword);

        await userRepository.Update(user);
    }
}