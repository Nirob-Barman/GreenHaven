using MediatR;

namespace CleanArchitecture.Application.Features.Products.Commands.DeleteProductImage;

public sealed record DeleteProductImageCommand(int ProductId) : IRequest<Unit>;
