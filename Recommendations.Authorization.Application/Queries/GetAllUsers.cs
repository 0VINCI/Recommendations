using Recommendations.Authorization.Core.Types;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Authorization.Application.Queries;

public sealed record GetAllUsers() : IQuery<IReadOnlyCollection<User>>;