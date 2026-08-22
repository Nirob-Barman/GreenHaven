using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Categories.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Categories.Queries.GetCategories;

public sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyCollection<CategoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Categories.AsNoTracking();
        if (!request.IncludeInactive)
        {
            query = query.Where(x => x.IsActive);
        }

        return await query
            .OrderBy(x => x.Name)
            .Select(x => new CategoryDto(
                x.Id,
                x.Name,
                x.Slug,
                x.Description,
                x.ImageUrl,
                x.IsActive,
                x.Products.Count))
            .ToListAsync(cancellationToken);
    }
}
