using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Purchase.Shared.Commands;

public sealed record AddNewCustomer(string dto, CancellationToken CancellationToken) : ICommand;