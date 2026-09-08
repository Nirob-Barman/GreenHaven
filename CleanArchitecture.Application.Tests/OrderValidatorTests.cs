using CleanArchitecture.Application.Features.Orders.Commands.CreateOrder;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Tests;

public sealed class OrderValidatorTests
{
    private readonly CreateOrderCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCashOnDeliveryOrder_ReturnsSuccess()
    {
        var command = new CreateOrderCommand(
            "cart-1", "Customer", "customer@example.com", "01700000000", "Dhaka", null,
            PaymentMethod.CashOnDelivery);

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithUnsupportedPaymentMethod_ReturnsFailure()
    {
        var command = new CreateOrderCommand(
            "cart-1", "Customer", "customer@example.com", "01700000000", "Dhaka", null,
            PaymentMethod.Stripe);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateOrderCommand.PaymentMethod));
    }
}
