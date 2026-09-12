using CleanArchitecture.Application.Features.Carts.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Carts.Commands.MergeCart;

public sealed record MergeCartCommand(string CartKey) : IRequest<CartDto>;
