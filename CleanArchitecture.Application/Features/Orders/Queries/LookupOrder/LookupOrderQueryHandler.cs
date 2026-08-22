using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Orders.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Orders.Queries.LookupOrder;

public sealed class LookupOrderQueryHandler : IRequestHandler<LookupOrderQuery, OrderDetailsDto>
{
    private readonly IApplicationDbContext _context;

    public LookupOrderQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDetailsDto> Handle(LookupOrderQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Orders.AsNoTracking().Where(x => x.OrderNumber == request.OrderNumber);

        if (!string.IsNullOrWhiteSpace(request.CustomerPhone))
        {
            query = query.Where(x => x.CustomerPhone == request.CustomerPhone);
        }

        if (!string.IsNullOrWhiteSpace(request.CustomerEmail))
        {
            query = query.Where(x => x.CustomerEmail == request.CustomerEmail);
        }

        var order = await query.FirstOrDefaultAsync(cancellationToken);
        if (order is null)
        {
            throw new NotFoundException(nameof(OrderDetailsDto), request.OrderNumber);
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
