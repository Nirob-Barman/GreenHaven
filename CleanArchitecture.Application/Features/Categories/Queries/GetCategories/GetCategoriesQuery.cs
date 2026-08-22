using CleanArchitecture.Application.Features.Categories.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Categories.Queries.GetCategories;

public sealed record GetCategoriesQuery(bool IncludeInactive = false) : IRequest<IReadOnlyCollection<CategoryDto>>;
