namespace CleanArchitecture.Application.Features.Products.Common;

public sealed record ProductDetailsDto(
    int Id,
    string Title,
    string Slug,
    string Description,
    decimal Price,
    int QuantityInStock,
    decimal Rating,
    int CategoryId,
    string CategoryName,
    string PrimaryImageUrl,
    bool IsActive);
