using CleanArchitecture.Application.Features.Categories.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Categories.Queries.GetCategoryBySlug;

public sealed record GetCategoryBySlugQuery(string Slug) : IRequest<CategoryDto>;
