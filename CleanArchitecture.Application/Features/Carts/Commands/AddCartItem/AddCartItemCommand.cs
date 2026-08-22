using CleanArchitecture.Application.Features.Carts.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Carts.Commands.AddCartItem;

public sealed record AddCartItemCommand(
    string CartKey,
    int ProductId,
    int Quantity) : IRequest<CartDto>;
