using CleanArchitecture.Application.Features.Orders.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Orders.Commands.CancelOrder;

public sealed record CancelOrderCommand(int Id) : IRequest<OrderDetailsDto>;
