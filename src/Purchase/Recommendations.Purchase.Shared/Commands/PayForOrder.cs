using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Purchase.Shared.Commands;

public sealed record PayForOrder(Guid OrderId, uint Method, DateTime PaymentDate, string Details) : ICommand;