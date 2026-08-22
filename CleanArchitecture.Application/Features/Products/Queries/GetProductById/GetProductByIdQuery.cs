using CleanArchitecture.Application.Features.Products.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(int Id) : IRequest<ProductDetailsDto>;
