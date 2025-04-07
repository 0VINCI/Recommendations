using Recommendations.Authorization.Core.Types;
using Recommendations.Shared.Abstractions;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Authorization.Core.Queries;

public sealed record GetUsers() : IQuery<IReadOnlyCollection<User>>;