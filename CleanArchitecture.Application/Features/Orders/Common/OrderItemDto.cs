using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Features.Orders.Common;

public sealed record OrderItemDto(
    int ProductId,
    string ProductTitle,
    string ProductImageUrl,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal);
