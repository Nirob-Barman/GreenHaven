using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Features.Orders.Commands.CancelOrder;
using CleanArchitecture.Application.Features.Orders.Commands.CreateOrder;
using CleanArchitecture.Application.Features.Orders.Commands.UpdateOrderStatus;
using CleanArchitecture.Application.Features.Orders.Common;
using CleanArchitecture.Application.Features.Orders.Queries.GetMyOrders;
using CleanArchitecture.Application.Features.Orders.Queries.GetOrderById;
using CleanArchitecture.Application.Features.Orders.Queries.GetOrders;
using CleanArchitecture.Application.Features.Orders.Queries.LookupOrder;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDetailsDto>> Create(
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<IReadOnlyCollection<OrderListItemDto>>> MyOrders(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetMyOrdersQuery(), cancellationToken));
    }

    [HttpGet("lookup")]
    public async Task<ActionResult<OrderDetailsDto>> Lookup(
        [FromQuery] string orderNumber,
        [FromQuery] string? customerPhone,
        [FromQuery] string? customerEmail,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new LookupOrderQuery(orderNumber, customerPhone, customerEmail), cancellationToken));
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpGet]
    public async Task<ActionResult<PaginatedList<OrderListItemDto>>> GetAll(
        [FromQuery] GetOrdersQuery query,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDetailsDto>> GetById(int id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetOrderByIdQuery(id), cancellationToken));
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<OrderDetailsDto>> UpdateStatus(
        int id,
        [FromBody] UpdateOrderStatusCommand command,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command with { Id = id }, cancellationToken));
    }

    [Authorize]
    [HttpPost("{id:int}/cancel")]
    public async Task<ActionResult<OrderDetailsDto>> Cancel(int id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new CancelOrderCommand(id), cancellationToken));
    }
}
