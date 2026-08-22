using CleanArchitecture.Application.Features.Products.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Products.Queries.GetProductBySlug;

public sealed record GetProductBySlugQuery(string Slug) : IRequest<ProductDetailsDto>;
