using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Carts.Common;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Carts.Commands.AddCartItem;

public sealed class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, CartDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public AddCartItemCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<CartDto> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ProductId && x.IsActive, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(Product), request.ProductId);
        }

        if (request.Quantity > product.QuantityInStock)
        {
            throw new DomainException("Requested quantity exceeds available stock.");
        }

        var cart = await LoadOrCreateCartAsync(request.CartKey, cancellationToken);
        EnsureCartOwnership(cart);

        var existingItem = await _context.CartItems
            .FirstOrDefaultAsync(x => x.CartId == cart.Id && x.ProductId == request.ProductId, cancellationToken);

        if (existingItem is null)
        {
            _context.CartItems.Add(CartItem.Create(request.ProductId, request.Quantity, product.Price));
        }
        else
        {
            var newQuantity = existingItem.Quantity + request.Quantity;
            if (newQuantity > product.QuantityInStock)
            {
                throw new DomainException("Requested quantity exceeds available stock.");
            }

            existingItem.ChangeQuantity(newQuantity);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return await LoadCartDtoAsync(cart.CartKey, cancellationToken);
    }

    private async Task<Cart> LoadOrCreateCartAsync(string cartKey, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .FirstOrDefaultAsync(x => x.CartKey == cartKey, cancellationToken);

        if (cart is not null)
        {
            return cart;
        }

        cart = Cart.Create(cartKey, _currentUser.IsAuthenticated ? _currentUser.UserId : null);
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync(cancellationToken);
        return cart;
    }

    private void EnsureCartOwnership(Cart cart)
    {
        if (_currentUser.IsAuthenticated && !string.IsNullOrWhiteSpace(_currentUser.UserId))
        {
            if (cart.UserId is null)
            {
                cart.AttachUser(_currentUser.UserId);
                return;
            }

            if (!string.Equals(cart.UserId, _currentUser.UserId, StringComparison.Ordinal))
            {
                throw new ForbiddenAccessException();
            }
        }
    }

    private async Task<CartDto> LoadCartDtoAsync(string cartKey, CancellationToken cancellationToken)
    {
        var query = _context.Carts
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
                x.Items.Sum(item => item.Quantity)));

        return await query.FirstAsync(cancellationToken);
    }
}
