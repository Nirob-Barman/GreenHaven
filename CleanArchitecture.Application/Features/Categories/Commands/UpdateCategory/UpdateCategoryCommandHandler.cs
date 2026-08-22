using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Categories.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (category is null)
        {
            throw new NotFoundException(nameof(CategoryDto), request.Id);
        }

        var slug = Slugify(request.Name);
        var duplicate = await _context.Categories.AnyAsync(x => x.Id != request.Id && (x.Name == request.Name || x.Slug == slug), cancellationToken);
        if (duplicate)
        {
            throw new InvalidOperationException("Category name or slug already exists.");
        }

        category.Update(request.Name, slug, request.Description, request.ImageUrl, request.ImagePublicId);
        await _context.SaveChangesAsync(cancellationToken);

        var count = await _context.Products.CountAsync(x => x.CategoryId == category.Id, cancellationToken);
        return new CategoryDto(category.Id, category.Name, category.Slug, category.Description, category.ImageUrl, category.IsActive, count);
    }

    private static string Slugify(string value)
    {
        return value.Trim().ToLowerInvariant().Replace(" ", "-");
    }
}
