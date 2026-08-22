using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Features.Products.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginatedList<ProductListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ProductListItemDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Include(x => x.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(x => x.Title.Contains(search) || x.Description.Contains(search));
        }

        if (request.CategoryId is not null)
        {
            query = query.Where(x => x.CategoryId == request.CategoryId);
        }

        if (!string.IsNullOrWhiteSpace(request.CategorySlug))
        {
            query = query.Where(x => x.Category != null && x.Category.Slug == request.CategorySlug);
        }

        if (request.MinPrice is not null)
        {
            query = query.Where(x => x.Price >= request.MinPrice);
        }

        if (request.MaxPrice is not null)
        {
            query = query.Where(x => x.Price <= request.MaxPrice);
        }

        if (request.MinRating is not null)
        {
            query = query.Where(x => x.Rating >= request.MinRating);
        }

        if (request.InStockOnly)
        {
            query = query.Where(x => x.QuantityInStock > 0);
        }

        query = ApplySorting(query, request);

        var projected = query.Select(x => new ProductListItemDto(
            x.Id,
            x.Title,
            x.Slug,
            x.Category!.Name,
            x.Price,
            x.QuantityInStock,
            x.Rating,
            x.PrimaryImageUrl,
            x.IsActive));

        return await PaginatedList<ProductListItemDto>.CreateAsync(
            projected,
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }

    private static IQueryable<Domain.Entities.Product> ApplySorting(
        IQueryable<Domain.Entities.Product> query,
        GetProductsQuery request)
    {
        var descending = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        return (request.SortBy?.ToLowerInvariant(), descending) switch
        {
            ("price", true) => query.OrderByDescending(x => x.Price),
            ("price", false) => query.OrderBy(x => x.Price),
            ("rating", true) => query.OrderByDescending(x => x.Rating),
            ("rating", false) => query.OrderBy(x => x.Rating),
            ("name", true) => query.OrderByDescending(x => x.Title),
            ("name", false) => query.OrderBy(x => x.Title),
            ("newest", true) => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderBy(x => x.CreatedAt)
        };
    }
}
