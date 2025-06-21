using Recommendations.Cart.Shared.DTO;
using Recommendations.Shared.Abstractions.Queries;

namespace Recommendations.Cart.Shared.Queries;

public sealed record GetUserCart() : IQuery<ShoppingCartDto?>; 