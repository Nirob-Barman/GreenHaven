using MediatR;

namespace CleanArchitecture.Application.Features.Products.Commands.UpdateProductStock;

public sealed record UpdateProductStockCommand(int Id, int QuantityInStock) : IRequest<int>;
