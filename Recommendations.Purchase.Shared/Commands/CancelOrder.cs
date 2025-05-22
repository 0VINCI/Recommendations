using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Purchase.Shared.Commands;

public sealed record CancelOrder(Guid OrderId) : ICommand;