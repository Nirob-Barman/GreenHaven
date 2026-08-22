using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities;

public sealed class OrderItem : BaseEntity
{
    public int OrderId { get; private set; }

    public Order? Order { get; private set; }

    public int ProductId { get; private set; }

    public string ProductTitleSnapshot { get; private set; } = string.Empty;

    public string ProductImageUrlSnapshot { get; private set; } = string.Empty;

    public decimal UnitPrice { get; private set; }

    public int Quantity { get; private set; }

    public decimal LineTotal { get; private set; }

    private OrderItem()
    {
    }

    public static OrderItem Create(
        int productId,
        string productTitleSnapshot,
        string productImageUrlSnapshot,
        decimal unitPrice,
        int quantity)
    {
        return new OrderItem
        {
            ProductId = productId,
            ProductTitleSnapshot = productTitleSnapshot,
            ProductImageUrlSnapshot = productImageUrlSnapshot,
            UnitPrice = unitPrice,
            Quantity = quantity,
            LineTotal = unitPrice * quantity
        };
    }
}
