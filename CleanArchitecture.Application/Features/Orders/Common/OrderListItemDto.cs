using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Features.Orders.Common;

public sealed record OrderListItemDto(
    int Id,
    string OrderNumber,
    string CustomerName,
    string CustomerPhone,
    decimal Total,
    PaymentMethod PaymentMethod,
    PaymentStatus PaymentStatus,
    OrderStatus Status,
    DateTimeOffset CreatedAt,
    int ItemCount);
