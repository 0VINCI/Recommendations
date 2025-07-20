using Recommendations.Authorization.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Authorization.Shared.Queries;

public sealed record GetUserById() : IQuery<UserInfoDto>;