namespace CleanArchitecture.Application.Features.Products.Common;

public sealed record ProductListItemDto(
    int Id,
    string Title,
    string Slug,
    string CategoryName,
    decimal Price,
    int QuantityInStock,
    decimal Rating,
    string PrimaryImageUrl,
    bool IsActive);
