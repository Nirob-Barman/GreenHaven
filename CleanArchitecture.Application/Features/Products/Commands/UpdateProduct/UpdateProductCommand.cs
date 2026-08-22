using CleanArchitecture.Application.Features.Products.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    int Id,
    int CategoryId,
    string Title,
    string Description,
    decimal Price,
    int QuantityInStock,
    decimal Rating,
    string PrimaryImageUrl,
    string? PrimaryImagePublicId,
    bool IsActive) : IRequest<ProductDetailsDto>;
