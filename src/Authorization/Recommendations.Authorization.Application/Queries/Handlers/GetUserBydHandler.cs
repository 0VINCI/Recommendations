using Recommendations.Authorization.Infrastructure.DAL.Repositories;
using Recommendations.Authorization.Shared.DTO;
using Recommendations.Authorization.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;
using Recommendations.Shared.Abstractions.UserContext;

namespace Recommendations.Authorization.Application.Queries.Handlers;

public sealed class GetUserBydHandler(IUserRepository userRepository,
    IUserContext userContext) : IQueryHandler<GetUserById, UserInfoDto>
{
    public async Task<UserInfoDto> HandleAsync(
        GetUserById query,
        CancellationToken cancellationToken = default)
    {
        var userId = userContext.UserId;
        var user = await userRepository.GetUserById(userId);
        
        return new UserInfoDto(user.IdUser, user.Name, user.Surname, user.Email);
    }
}