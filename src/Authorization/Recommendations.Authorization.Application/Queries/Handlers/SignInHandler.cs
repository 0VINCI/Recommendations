using Recommendations.Authorization.Core.Exceptions;
using Recommendations.Authorization.Infrastructure.DAL.Repositories;
using Recommendations.Authorization.Shared.DTO;
using Recommendations.Authorization.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.Abstractions.Services;

namespace Recommendations.Authorization.Application.Queries.Handlers;

public sealed class SignInHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService) : IQueryHandler<SignIn, SignedInDto>
{
    public async Task<SignedInDto> HandleAsync(SignIn query, CancellationToken cancellationToken = default)
    {
        var data = query.SignInDto;

        var user = await userRepository.GetUserByEmail(data.Email);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        if (data.Password != user.Password)
        {
            throw new InvalidPasswordException();
        }
        var token = jwtTokenService.GenerateToken(user.IdUser.ToString(), user.Email);

        return new SignedInDto(user.IdUser, user.Name, user.Surname, user.Email, token);
    } 
}