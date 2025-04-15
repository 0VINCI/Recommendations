using Recommendations.Authorization.Core.Types;
using Recommendations.Authorization.Infrastructure.DAL.Repositories;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Authorization.Application.Queries.Handlers;

public sealed class GetAllUsersHandler(IUserRepository userRepository) : IQueryHandler<GetAllUsers, IReadOnlyCollection<User>>
{
    public async Task<IReadOnlyCollection<User>> HandleAsync(
        GetAllUsers query,
        CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllUsers();
        return users;
    }
}