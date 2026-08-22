using CleanArchitecture.Application.Features.Categories.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(
    string Name,
    string? Description,
    string? ImageUrl,
    string? ImagePublicId) : IRequest<CategoryDto>;
