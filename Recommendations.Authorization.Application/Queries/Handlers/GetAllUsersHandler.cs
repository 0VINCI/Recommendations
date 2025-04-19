using AutoMapper;
using Recommendations.Authorization.Infrastructure.DAL.Repositories;
using Recommendations.Authorization.Shared.DTO;
using Recommendations.Authorization.Shared.Queries;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Authorization.Application.Queries.Handlers;

public sealed class GetAllUsersHandler(IUserRepository userRepository, IMapper mapper) : IQueryHandler<GetAllUsers, IReadOnlyCollection<UserDto>>
{
    public async Task<IReadOnlyCollection<UserDto>> HandleAsync(
        GetAllUsers query,
        CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllUsers();
        return users
            .Select(mapper.Map<UserDto>)
            .ToList();
    }
}