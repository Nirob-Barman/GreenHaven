using CleanArchitecture.Application.Features.Carts.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Carts.Commands.RemoveCartItem;

public sealed record RemoveCartItemCommand(string CartKey, int ProductId) : IRequest<CartDto>;
