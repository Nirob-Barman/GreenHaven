using CleanArchitecture.Application.Features.Categories.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(
    int Id,
    string Name,
    string? Description,
    string? ImageUrl,
    string? ImagePublicId) : IRequest<CategoryDto>;
