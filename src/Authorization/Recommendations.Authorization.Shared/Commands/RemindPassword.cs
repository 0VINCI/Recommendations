
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Authorization.Shared.Commands;

public sealed record RemindPassword(string Email) : ICommand;