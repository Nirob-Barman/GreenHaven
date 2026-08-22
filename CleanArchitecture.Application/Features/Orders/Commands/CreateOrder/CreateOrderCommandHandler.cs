using System.Security.Cryptography;
using System.Text;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Orders.Common;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public CreateOrderCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<OrderDetailsDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (request.PaymentMethod != PaymentMethod.CashOnDelivery)
        {
            throw new DomainException("Only Cash on Delivery is currently supported.");
        }

        await using var transaction = await _context.BeginTransactionAsync(cancellationToken);

        var cart = await _context.Carts
            .Include(x => x.Items)
                .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.CartKey == request.CartKey, cancellationToken);

        if (cart is null)
        {
            throw new NotFoundException(nameof(Cart), request.CartKey);
        }

        if (cart.Items.Count == 0)
        {
            throw new DomainException("Cart is empty.");
        }

        if (_currentUser.IsAuthenticated && !string.IsNullOrWhiteSpace(_currentUser.UserId))
        {
            if (cart.UserId is null)
            {
                cart.AttachUser(_currentUser.UserId);
            }
            else if (!string.Equals(cart.UserId, _currentUser.UserId, StringComparison.Ordinal))
            {
                throw new ForbiddenAccessException();
            }
        }

        foreach (var item in cart.Items)
        {
            if (item.Product is null || !item.Product.IsActive)
            {
                throw new DomainException("One or more products in the cart are unavailable.");
            }

            if (item.Quantity > item.Product.QuantityInStock)
            {
                throw new DomainException("One or more products do not have enough stock.");
            }
        }

        var orderItems = cart.Items
            .Select(item => OrderItem.Create(
                item.ProductId,
                item.Product!.Title,
                item.Product.PrimaryImageUrl,
                item.Product.Price,
                item.Quantity))
            .ToList();

        var order = Order.Create(
            GenerateOrderNumber(),
            cart.UserId,
            request.CustomerName,
            request.CustomerEmail,
            request.CustomerPhone,
            request.ShippingAddress,
            request.Notes,
            deliveryCharge: 0m,
            PaymentMethod.CashOnDelivery,
            PaymentStatus.Unpaid,
            orderItems);

        _context.Orders.Add(order);

        foreach (var item in cart.Items)
        {
            item.Product!.ChangeStock(item.Product.QuantityInStock - item.Quantity);
        }

        _context.CartItems.RemoveRange(cart.Items);

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return await LoadOrderDetailsAsync(order.Id, cancellationToken);
    }

    private async Task<OrderDetailsDto> LoadOrderDetailsAsync(int orderId, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(x => x.Id == orderId)
            .Select(x => new OrderDetailsDto(
                x.Id,
                x.OrderNumber,
                x.UserId,
                x.CustomerName,
                x.CustomerEmail,
                x.CustomerPhone,
                x.ShippingAddress,
                x.Notes,
                x.Subtotal,
                x.DeliveryCharge,
                x.Total,
                x.PaymentMethod,
                x.PaymentStatus,
                x.Status,
                x.Items.Select(item => new OrderItemDto(
                    item.ProductId,
                    item.ProductTitleSnapshot,
                    item.ProductImageUrlSnapshot,
                    item.UnitPrice,
                    item.Quantity,
                    item.LineTotal)).ToList(),
                x.CreatedAt))
            .FirstAsync(cancellationToken);
    }

    private static string GenerateOrderNumber()
    {
        var suffix = RandomNumberGenerator.GetInt32(1000, 9999);
        return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{suffix}";
    }
}
