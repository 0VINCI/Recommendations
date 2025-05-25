using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Purchase.Shared.Commands;

public sealed record UpdateStatus(Guid OrderId, uint Status) : ICommand;