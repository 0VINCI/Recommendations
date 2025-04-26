using Recommendations.Shared.Abstractions.Commands;

namespace Recommendations.Cart.Shared.Commands;

public sealed record RemoveItemFromCart(Guid ProductId) : ICommand;