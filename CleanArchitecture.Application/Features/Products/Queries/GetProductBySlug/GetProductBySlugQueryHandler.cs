using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Products.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Products.Queries.GetProductBySlug;

public sealed class GetProductBySlugQueryHandler : IRequestHandler<GetProductBySlugQuery, ProductDetailsDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductBySlugQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDetailsDto> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Products
            .AsNoTracking()
            .Where(x => x.Slug == request.Slug && x.IsActive)
            .Select(x => new ProductDetailsDto(
                x.Id,
                x.Title,
                x.Slug,
                x.Description,
                x.Price,
                x.QuantityInStock,
                x.Rating,
                x.CategoryId,
                x.Category!.Name,
                x.PrimaryImageUrl,
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        return result ?? throw new NotFoundException(nameof(ProductDetailsDto), request.Slug);
    }
}
