using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Carts.Common;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Carts.Commands.RemoveCartItem;

public sealed class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, CartDto>
{
    private readonly IApplicationDbContext _context;

    public RemoveCartItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CartDto> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(x => x.CartKey == request.CartKey, cancellationToken);
        if (cart is null)
        {
            throw new NotFoundException(nameof(Cart), request.CartKey);
        }

        var item = await _context.CartItems.FirstOrDefaultAsync(x => x.CartId == cart.Id && x.ProductId == request.ProductId, cancellationToken);
        if (item is null)
        {
            throw new NotFoundException(nameof(CartItem), request.ProductId);
        }

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
        return await LoadCartDtoAsync(cart.CartKey, cancellationToken);
    }

    private async Task<CartDto> LoadCartDtoAsync(string cartKey, CancellationToken cancellationToken)
    {
        return await _context.Carts
            .AsNoTracking()
            .Where(x => x.CartKey == cartKey)
            .Select(x => new CartDto(
                x.CartKey,
                x.Items.Select(item => new CartItemDto(
                    item.ProductId,
                    item.Product!.Title,
                    item.Product.Slug,
                    item.Product.PrimaryImageUrl,
                    item.UnitPriceSnapshot,
                    item.Quantity,
                    item.UnitPriceSnapshot * item.Quantity,
                    item.Product.QuantityInStock)).ToList(),
                x.Items.Sum(item => item.UnitPriceSnapshot * item.Quantity),
                x.Items.Sum(item => item.Quantity)))
            .FirstAsync(cancellationToken);
    }
}
