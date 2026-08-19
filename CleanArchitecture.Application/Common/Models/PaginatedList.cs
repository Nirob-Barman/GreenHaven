namespace CleanArchitecture.Application.Common.Models;

public sealed class PaginatedList<T>
{
    public PaginatedList(
        IReadOnlyCollection<T> items,
        int count,
        int pageNumber,
        int pageSize)
    {
        Items = items;
        TotalCount = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    public IReadOnlyCollection<T> Items { get; }

    public int PageNumber { get; }

    public int TotalPages { get; }

    public int TotalCount { get; }

    public int PageSize { get; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}
