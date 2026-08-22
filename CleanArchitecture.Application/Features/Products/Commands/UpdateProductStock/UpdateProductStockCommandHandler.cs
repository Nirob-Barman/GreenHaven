using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Products.Commands.UpdateProductStock;

public sealed class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, int>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductStockCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        product.ChangeStock(request.QuantityInStock);
        await _context.SaveChangesAsync(cancellationToken);
        return product.QuantityInStock;
    }
}
