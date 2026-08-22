using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Categories.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Categories.Queries.GetCategoryBySlug;

public sealed class GetCategoryBySlugQueryHandler : IRequestHandler<GetCategoryBySlugQuery, CategoryDto>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryBySlugQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDto> Handle(GetCategoryBySlugQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Categories
            .AsNoTracking()
            .Where(x => x.Slug == request.Slug)
            .Select(x => new CategoryDto(
                x.Id,
                x.Name,
                x.Slug,
                x.Description,
                x.ImageUrl,
                x.IsActive,
                x.Products.Count))
            .FirstOrDefaultAsync(cancellationToken);

        return result ?? throw new NotFoundException(nameof(CategoryDto), request.Slug);
    }
}
