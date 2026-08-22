using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Carts.Common;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Carts.Queries.GetCart;

public sealed class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartDto>
{
    private readonly IApplicationDbContext _context;

    public GetCartQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CartDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts.AsNoTracking().FirstOrDefaultAsync(x => x.CartKey == request.CartKey, cancellationToken);
        if (cart is null)
        {
            throw new NotFoundException(nameof(Cart), request.CartKey);
        }

        return await _context.Carts
            .AsNoTracking()
            .Where(x => x.CartKey == request.CartKey)
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
