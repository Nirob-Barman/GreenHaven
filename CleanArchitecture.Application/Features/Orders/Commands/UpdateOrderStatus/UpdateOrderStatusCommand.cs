using CleanArchitecture.Application.Features.Orders.Common;
using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.Features.Orders.Commands.UpdateOrderStatus;

public sealed record UpdateOrderStatusCommand(
    int Id,
    OrderStatus Status) : IRequest<OrderDetailsDto>;
