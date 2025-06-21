using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Cart.Shared.Commands;

public sealed record UpdateCartItemQuantity(Guid ProductId, int Quantity) : ICommand;