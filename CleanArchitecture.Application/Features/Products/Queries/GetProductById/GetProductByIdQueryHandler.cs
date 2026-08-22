using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Products.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDetailsDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDetailsDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Products
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.IsActive)
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

        return result ?? throw new NotFoundException(nameof(ProductDetailsDto), request.Id);
    }
}
