using CleanArchitecture.Application.Features.Orders.Common;
using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.Features.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    string CartKey,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    string ShippingAddress,
    string? Notes,
    PaymentMethod PaymentMethod) : IRequest<OrderDetailsDto>;
