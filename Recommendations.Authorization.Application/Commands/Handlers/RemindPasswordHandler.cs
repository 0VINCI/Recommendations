using Recommendations.Authorization.Application.Exceptions;
using Recommendations.Authorization.Infrastructure.DAL.Repositories;
using Recommendations.Authorization.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Authorization.Application.Commands.Handlers;

internal sealed class RemindPasswordHandler(IUserRepository userRepository) : ICommandHandler<RemindPassword>
{
    public async Task HandleAsync(RemindPassword command, CancellationToken cancellationToken = default)
    {
        var data = command.Email;

        var user = await userRepository.ExistsByEmailAsync(data);

        if (!user)
        {
            throw new UserNotFoundException();
        }
    }
}