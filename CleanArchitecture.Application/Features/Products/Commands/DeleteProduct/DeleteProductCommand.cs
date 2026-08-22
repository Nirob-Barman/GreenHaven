using MediatR;

namespace CleanArchitecture.Application.Features.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(int Id) : IRequest<Unit>;
