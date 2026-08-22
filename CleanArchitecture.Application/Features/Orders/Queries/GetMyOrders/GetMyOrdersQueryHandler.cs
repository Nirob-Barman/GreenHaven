using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Orders.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Orders.Queries.GetMyOrders;

public sealed class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, IReadOnlyCollection<OrderListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GetMyOrdersQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyCollection<OrderListItemDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
        {
            throw new ForbiddenAccessException();
        }

        return await _context.Orders
            .AsNoTracking()
            .Where(x => x.UserId == _currentUser.UserId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new OrderListItemDto(
                x.Id,
                x.OrderNumber,
                x.CustomerName,
                x.CustomerPhone,
                x.Total,
                x.PaymentMethod,
                x.PaymentStatus,
                x.Status,
                x.CreatedAt,
                x.Items.Count))
            .ToListAsync(cancellationToken);
    }
}
