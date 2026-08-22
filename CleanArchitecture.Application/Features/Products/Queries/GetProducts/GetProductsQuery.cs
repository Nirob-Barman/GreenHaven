using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Features.Products.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductsQuery : PaginationQuery, IRequest<PaginatedList<ProductListItemDto>>
{
    public string? Search { get; init; }

    public int? CategoryId { get; init; }

    public string? CategorySlug { get; init; }

    public decimal? MinPrice { get; init; }

    public decimal? MaxPrice { get; init; }

    public decimal? MinRating { get; init; }

    public bool InStockOnly { get; init; }

    public string SortBy { get; init; } = "newest";

    public string SortDirection { get; init; } = "desc";
}
