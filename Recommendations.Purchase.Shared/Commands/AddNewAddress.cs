using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Purchase.Shared.Commands;

public sealed record AddNewAddress(string dto, CancellationToken CancellationToken) : ICommand;