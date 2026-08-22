using CleanArchitecture.Application.Features.Orders.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Orders.Queries.GetMyOrders;

public sealed record GetMyOrdersQuery : IRequest<IReadOnlyCollection<OrderListItemDto>>;
