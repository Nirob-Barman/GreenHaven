using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Features.Orders.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Orders.Queries.GetOrders;

public sealed class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PaginatedList<OrderListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetOrdersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<OrderListItemDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Orders.AsNoTracking().AsQueryable();

        if (request.Status is not null)
        {
            query = query.Where(x => x.Status == request.Status);
        }

        if (request.PaymentStatus is not null)
        {
            query = query.Where(x => x.PaymentStatus == request.PaymentStatus);
        }

        if (request.PaymentMethod is not null)
        {
            query = query.Where(x => x.PaymentMethod == request.PaymentMethod);
        }

        if (request.FromDate is not null)
        {
            query = query.Where(x => x.CreatedAt >= request.FromDate);
        }

        if (request.ToDate is not null)
        {
            query = query.Where(x => x.CreatedAt <= request.ToDate);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(x => x.OrderNumber.Contains(search) || x.CustomerName.Contains(search) || x.CustomerPhone.Contains(search));
        }

        var projected = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new OrderListItemDto(
                x.Id,
                x.OrderNumber,
                x.CustomerName,
                x.CustomerPhone,
                x.Total,
                x.PaymentMethod,
                x.PaymentStatus,
                x.Status,
                x.CreatedAt,
                x.Items.Count));

        return await PaginatedList<OrderListItemDto>.CreateAsync(
            projected,
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}
