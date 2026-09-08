using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Tests;

public sealed class OrderTests
{
    [Fact]
    public void Create_CalculatesSubtotalAndTotalFromItems()
    {
        var items = new[]
        {
            OrderItem.Create(1, "Fern", "image", 12.50m, 2),
            OrderItem.Create(2, "Cactus", "image", 5m, 1)
        };

        var order = Order.Create(
            "GH-001", "user-1", "Customer", "customer@example.com", "01700000000", "Dhaka", null,
            60m, PaymentMethod.CashOnDelivery, PaymentStatus.Pending, items);

        Assert.Equal(30m, order.Subtotal);
        Assert.Equal(90m, order.Total);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void Cancel_WhenPending_ChangesStatusToCancelled()
    {
        var order = CreateOrder();

        order.Cancel();

        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }

    [Fact]
    public void Cancel_WhenAlreadyDelivered_ThrowsDomainException()
    {
        var order = CreateOrder();
        order.ChangeStatus(OrderStatus.Confirmed);
        order.ChangeStatus(OrderStatus.Shipped);
        order.ChangeStatus(OrderStatus.Delivered);

        Assert.Throws<DomainException>(() => order.Cancel());
    }

    private static Order CreateOrder() => Order.Create(
        "GH-001", "user-1", "Customer", "customer@example.com", "01700000000", "Dhaka", null,
        60m, PaymentMethod.CashOnDelivery, PaymentStatus.Pending,
        [OrderItem.Create(1, "Fern", "image", 12.50m, 2)]);
}
