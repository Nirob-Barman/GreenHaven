using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Carts.Commands.ClearCart;

public sealed class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public ClearCartCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(x => x.CartKey == request.CartKey, cancellationToken);
        if (cart is null)
        {
            throw new NotFoundException(nameof(Cart), request.CartKey);
        }

        var items = await _context.CartItems.Where(x => x.CartId == cart.Id).ToListAsync(cancellationToken);
        _context.CartItems.RemoveRange(items);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
