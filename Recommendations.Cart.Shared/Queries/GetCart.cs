using Recommendations.Cart.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Cart.Shared.Queries;

public sealed record GetCart(Guid CartId) : IQuery<ShoppingCartDto?>;