using System.Collections.ObjectModel;
using Recommendations.Cart.Core.Exceptions;

namespace Recommendations.Cart.Core.Types;

public sealed class ShoppingCart(Guid userId)
{
    public Guid IdCart { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; } = userId;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private readonly List<CartItem> _items = new();
    
    public IReadOnlyCollection<CartItem> Items => new ReadOnlyCollection<CartItem>(_items);

    public decimal Total => _items.Sum(i => i.Subtotal);
    
    public void AddItem(Guid productId, string name, decimal price, int qty = 1)
    {
        var existing = _items.SingleOrDefault(i => i.ProductId == productId);
        if (existing is not null)
            existing.IncreaseQuantity(qty);
        else
            _items.Add(new CartItem(productId, name, price, qty));
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.SingleOrDefault(i => i.ProductId == productId)
                   ?? throw new ItemNotInCartException();
        _items.Remove(item);
    }

    public void UpdateItemQuantity(Guid productId, int newQty)
    {
        var item = _items.SingleOrDefault(i => i.ProductId == productId)
                   ?? throw new ItemNotInCartException();
        item.ChangeQuantity(newQty);
    }
}