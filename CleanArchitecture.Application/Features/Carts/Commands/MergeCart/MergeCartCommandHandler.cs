using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Carts.Common;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Carts.Commands.MergeCart;

public sealed class MergeCartCommandHandler : IRequestHandler<MergeCartCommand, CartDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public MergeCartCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<CartDto> Handle(MergeCartCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
        {
            throw new UnauthorizedAccessException();
        }

        var sourceCart = await _context.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.CartKey == request.CartKey, cancellationToken);

        if (sourceCart is null)
        {
            throw new NotFoundException(nameof(Cart), request.CartKey);
        }

        if (sourceCart.UserId is not null
            && !string.Equals(sourceCart.UserId, _currentUser.UserId, StringComparison.Ordinal))
        {
            throw new ForbiddenAccessException();
        }

        var destinationCart = await _context.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserId == _currentUser.UserId, cancellationToken);

        if (destinationCart is null || destinationCart.Id == sourceCart.Id)
        {
            sourceCart.AttachUser(_currentUser.UserId);
            await _context.SaveChangesAsync(cancellationToken);
            return await LoadCartDtoAsync(sourceCart.CartKey, cancellationToken);
        }

        var productIds = sourceCart.Items
            .Select(x => x.ProductId)
            .Concat(destinationCart.Items.Select(x => x.ProductId))
            .Distinct()
            .ToArray();

        var products = await _context.Products
            .Where(x => productIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (var sourceItem in sourceCart.Items.ToArray())
        {
            if (!products.TryGetValue(sourceItem.ProductId, out var product) || !product.IsActive)
            {
                throw new NotFoundException(nameof(Product), sourceItem.ProductId);
            }

            var destinationItem = destinationCart.Items
                .FirstOrDefault(x => x.ProductId == sourceItem.ProductId);

            var mergedQuantity = sourceItem.Quantity + (destinationItem?.Quantity ?? 0);
            if (mergedQuantity > product.QuantityInStock)
            {
                throw new DomainException(
                    $"Cannot merge product '{product.Title}' because the requested quantity exceeds available stock.");
            }

            if (destinationItem is null)
            {
                _context.CartItems.Add(CartItem.Create(
                    sourceItem.ProductId,
                    sourceItem.Quantity,
                    sourceItem.UnitPriceSnapshot));
            }
            else
            {
                destinationItem.ChangeQuantity(mergedQuantity);
            }
        }

        _context.CartItems.RemoveRange(sourceCart.Items);
        _context.Carts.Remove(sourceCart);
        await _context.SaveChangesAsync(cancellationToken);

        return await LoadCartDtoAsync(destinationCart.CartKey, cancellationToken);
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
