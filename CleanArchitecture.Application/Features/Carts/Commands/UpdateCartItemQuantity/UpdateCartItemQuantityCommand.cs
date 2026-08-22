using CleanArchitecture.Application.Features.Carts.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Carts.Commands.UpdateCartItemQuantity;

public sealed record UpdateCartItemQuantityCommand(
    string CartKey,
    int ProductId,
    int Quantity) : IRequest<CartDto>;
