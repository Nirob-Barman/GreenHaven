using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities;

public sealed class CartItem : BaseEntity
{
    public int CartId { get; private set; }

    public Cart? Cart { get; private set; }

    public int ProductId { get; private set; }

    public Product? Product { get; private set; }

    public int Quantity { get; private set; }

    public decimal UnitPriceSnapshot { get; private set; }

    private CartItem()
    {
    }

    public static CartItem Create(int productId, int quantity, decimal unitPriceSnapshot)
    {
        return new CartItem
        {
            ProductId = productId,
            Quantity = quantity,
            UnitPriceSnapshot = unitPriceSnapshot
        };
    }

    public void IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
        MarkUpdated();
    }

    public void ChangeQuantity(int quantity)
    {
        Quantity = quantity;
        MarkUpdated();
    }
}
