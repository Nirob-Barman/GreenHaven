namespace CleanArchitecture.Application.Features.Categories.Common;

public sealed record CategoryDto(
    int Id,
    string Name,
    string Slug,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    int ProductCount);
