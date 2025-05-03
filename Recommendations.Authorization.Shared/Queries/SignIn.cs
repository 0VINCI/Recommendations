using Recommendations.Authorization.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Authorization.Shared.Queries;

public record SignIn(SignInDto SignInDto) : IQuery<SignedInDto>;
