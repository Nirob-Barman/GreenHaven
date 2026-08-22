namespace CleanArchitecture.Application.Features.Carts.Common;

public sealed record CartItemDto(
    int ProductId,
    string ProductTitle,
    string ProductSlug,
    string ProductImageUrl,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal,
    int AvailableStock);
