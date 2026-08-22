using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Features.Orders.Common;

public sealed record OrderDetailsDto(
    int Id,
    string OrderNumber,
    string? UserId,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    string ShippingAddress,
    string? Notes,
    decimal Subtotal,
    decimal DeliveryCharge,
    decimal Total,
    PaymentMethod PaymentMethod,
    PaymentStatus PaymentStatus,
    OrderStatus Status,
    IReadOnlyCollection<OrderItemDto> Items,
    DateTimeOffset CreatedAt);
