using Recommendations.Authorization.Application.DTO;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Authorization.Application.Commands;

public record RemindPassword(string Email) : ICommand;