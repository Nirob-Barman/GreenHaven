using CleanArchitecture.Application.Features.Orders.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(int Id) : IRequest<OrderDetailsDto>;
