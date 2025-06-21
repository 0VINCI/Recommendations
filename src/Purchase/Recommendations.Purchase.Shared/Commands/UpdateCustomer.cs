using Recommendations.Purchase.Shared.DTO;
using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Purchase.Shared.Commands;

public sealed record UpdateCustomer(CustomerDto CustomerDto) : ICommand;