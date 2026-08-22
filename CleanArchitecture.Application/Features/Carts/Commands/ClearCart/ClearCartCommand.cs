using MediatR;

namespace CleanArchitecture.Application.Features.Carts.Commands.ClearCart;

public sealed record ClearCartCommand(string CartKey) : IRequest<Unit>;
