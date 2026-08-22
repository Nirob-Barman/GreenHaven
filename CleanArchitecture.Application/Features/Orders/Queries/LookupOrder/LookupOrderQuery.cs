using CleanArchitecture.Application.Features.Orders.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Orders.Queries.LookupOrder;

public sealed record LookupOrderQuery(
    string OrderNumber,
    string? CustomerPhone,
    string? CustomerEmail) : IRequest<OrderDetailsDto>;
