using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public sealed class Order : BaseEntity
{
    public string OrderNumber { get; private set; } = string.Empty;

    public string? UserId { get; private set; }

    public string CustomerName { get; private set; } = string.Empty;

    public string CustomerEmail { get; private set; } = string.Empty;

    public string CustomerPhone { get; private set; } = string.Empty;

    public string ShippingAddress { get; private set; } = string.Empty;

    public string? Notes { get; private set; }

    public decimal Subtotal { get; private set; }

    public decimal DeliveryCharge { get; private set; }

    public decimal Total { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; }

    public PaymentStatus PaymentStatus { get; private set; }

    public OrderStatus Status { get; private set; }

    public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

    private Order()
    {
    }

    public static Order Create(
        string orderNumber,
        string? userId,
        string customerName,
        string customerEmail,
        string customerPhone,
        string shippingAddress,
        string? notes,
        decimal deliveryCharge,
        PaymentMethod paymentMethod,
        PaymentStatus paymentStatus,
        IEnumerable<OrderItem> items)
    {
        var order = new Order
        {
            OrderNumber = orderNumber,
            UserId = userId,
            CustomerName = customerName,
            CustomerEmail = customerEmail,
            CustomerPhone = customerPhone,
            ShippingAddress = shippingAddress,
            Notes = notes,
            DeliveryCharge = deliveryCharge,
            PaymentMethod = paymentMethod,
            PaymentStatus = paymentStatus,
            Status = OrderStatus.Pending,
            Items = items.ToList()
        };

        order.Subtotal = order.Items.Sum(item => item.LineTotal);
        order.Total = order.Subtotal + order.DeliveryCharge;

        return order;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Delivered or OrderStatus.Cancelled)
        {
            throw new DomainException("This order cannot be cancelled.");
        }

        Status = OrderStatus.Cancelled;
        MarkUpdated();
    }

    public void ChangeStatus(OrderStatus status)
    {
        if (Status is OrderStatus.Cancelled or OrderStatus.Delivered)
        {
            throw new DomainException("This order status cannot be changed.");
        }

        Status = status;
        MarkUpdated();
    }

    public void MarkPaid()
    {
        PaymentStatus = PaymentStatus.Paid;
        MarkUpdated();
    }
}
