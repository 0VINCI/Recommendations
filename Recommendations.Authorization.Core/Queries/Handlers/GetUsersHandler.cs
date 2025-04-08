using Recommendations.Authorization.Core.Types;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Authorization.Core.Queries.Handlers;

internal sealed class GetUsersHandler : IQueryHandler<GetUsers, IReadOnlyCollection<User>>
{
    public async Task<IReadOnlyCollection<User>> HandleAsync(
        GetUsers query,
        CancellationToken cancellationToken = default)
    {
        var users = new List<User>
        {
            new User { Id = 1, Name = "Jan", Surname = "Kowalski", Email = "jan.kowalski@test.com" },
            new User { Id = 2, Name = "Anna", Surname = "Nowak", Email = "anna.nowak@test.com" }
        };

        return await Task.FromResult(users);
    }
}