using CleanArchitecture.Application.Features.Carts.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Carts.Queries.GetCart;

public sealed record GetCartQuery(string CartKey) : IRequest<CartDto>;
