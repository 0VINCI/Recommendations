using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Cart.Shared.Commands;

public sealed record AddItemToCart(Guid ProductId, int Quantity) : ICommand;