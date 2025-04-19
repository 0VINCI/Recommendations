using Recommendations.Authorization.Shared.DTO;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Authorization.Shared.Commands;

public record SignIn(SignInDto SignInDto) : ICommand;