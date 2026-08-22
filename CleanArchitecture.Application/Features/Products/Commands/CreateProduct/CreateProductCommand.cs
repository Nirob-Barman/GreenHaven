using CleanArchitecture.Application.Features.Products.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    int CategoryId,
    string Title,
    string Description,
    decimal Price,
    int QuantityInStock,
    decimal Rating,
    string PrimaryImageUrl,
    string? PrimaryImagePublicId) : IRequest<ProductDetailsDto>;
