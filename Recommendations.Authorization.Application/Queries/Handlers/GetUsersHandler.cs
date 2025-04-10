using Recommendations.Authorization.Core.Types;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Authorization.Core.Queries.Handlers;

public sealed class GetUsersHandler : IQueryHandler<GetUsers, IReadOnlyCollection<User>>
{
    public async Task<IReadOnlyCollection<User>> HandleAsync(
        GetUsers query,
        CancellationToken cancellationToken = default)
    {
        var users = new List<User>
        {
            new() { IdUser = new Guid(), Name = "Jan", Surname = "Kowalski", Email = "jan.kowalski@test.com", Password = "abc"},
            new() { IdUser = new Guid(), Name = "Anna", Surname = "Nowak", Email = "anna.nowak@test.com", Password = "abc" }
        };

        return await Task.FromResult(users);
    }
}