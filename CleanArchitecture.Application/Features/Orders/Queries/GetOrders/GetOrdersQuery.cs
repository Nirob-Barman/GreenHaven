using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Features.Orders.Common;
using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.Features.Orders.Queries.GetOrders;

public sealed class GetOrdersQuery : PaginationQuery, IRequest<PaginatedList<OrderListItemDto>>
{
    public OrderStatus? Status { get; init; }

    public PaymentStatus? PaymentStatus { get; init; }

    public PaymentMethod? PaymentMethod { get; init; }

    public DateTimeOffset? FromDate { get; init; }

    public DateTimeOffset? ToDate { get; init; }

    public string? Search { get; init; }
}
