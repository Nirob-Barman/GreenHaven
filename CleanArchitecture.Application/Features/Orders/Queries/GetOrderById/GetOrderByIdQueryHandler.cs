using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Orders.Common;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GetOrderByIdQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<OrderDetailsDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (order is null)
        {
            throw new NotFoundException(nameof(OrderDetailsDto), request.Id);
        }

        if (_currentUser.IsAuthenticated && !string.IsNullOrWhiteSpace(_currentUser.UserId) && !string.Equals(order.UserId, _currentUser.UserId, StringComparison.Ordinal))
        {
            throw new ForbiddenAccessException();
        }

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
}
