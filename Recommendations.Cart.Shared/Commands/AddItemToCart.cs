using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Cart.Shared.Commands;

public sealed record AddItemToCart(Guid ProductId, string Name, decimal Price, int Quantity) : ICommand;