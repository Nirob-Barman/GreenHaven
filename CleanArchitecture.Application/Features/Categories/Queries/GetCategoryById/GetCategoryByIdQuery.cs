using CleanArchitecture.Application.Features.Categories.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto>;
