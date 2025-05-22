using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Purchase.Shared.Commands;

public sealed record UpdateAddress(Guid OrderId, uint Status, CancellationToken CancellationToken) : ICommand;