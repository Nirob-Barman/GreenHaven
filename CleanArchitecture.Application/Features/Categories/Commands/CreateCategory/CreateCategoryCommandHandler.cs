using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Categories.Common;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var slug = Slugify(request.Name);
        var exists = await _context.Categories.AnyAsync(x => x.Name == request.Name || x.Slug == slug, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException("Category name or slug already exists.");
        }

        var category = Category.Create(request.Name, slug, request.Description, request.ImageUrl, request.ImagePublicId);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        return new CategoryDto(category.Id, category.Name, category.Slug, category.Description, category.ImageUrl, category.IsActive, 0);
    }

    private static string Slugify(string value)
    {
        return value.Trim().ToLowerInvariant().Replace(" ", "-");
    }
}
