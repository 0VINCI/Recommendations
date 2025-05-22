using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Purchase.Shared.Commands;

public sealed record UpdateCustomer(string dto, CancellationToken CancellationToken) : ICommand;