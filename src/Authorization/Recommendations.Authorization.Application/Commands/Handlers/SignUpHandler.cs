using Recommendations.Authorization.Application.Exceptions;
using Recommendations.Authorization.Core.Types;
using Recommendations.Authorization.Infrastructure.DAL.Repositories;
using Recommendations.Authorization.Shared.Commands;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Authorization.Application.Commands.Handlers;

internal sealed class SignUpHandler(IUserRepository userRepository) : ICommandHandler<SignUp>
{
    public async Task HandleAsync(SignUp command, CancellationToken cancellationToken = default)
    {
        var dataFromRequest = command.SignUpDto;

        var exists = await userRepository.ExistsByEmailAsync(dataFromRequest.Email);

        if (exists)
        {
            throw new UserAlreadyExistsException();
        }

        var user = User.Create(dataFromRequest.Name, dataFromRequest.Surname, dataFromRequest.Email,
            dataFromRequest.Password);
        
        await userRepository.AddAsync(user);
    }
}