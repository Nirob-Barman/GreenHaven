using CleanArchitecture.Application.Features.Carts.Commands.AddCartItem;
using CleanArchitecture.Application.Features.Carts.Commands.ClearCart;
using CleanArchitecture.Application.Features.Carts.Commands.RemoveCartItem;
using CleanArchitecture.Application.Features.Carts.Commands.MergeCart;
using CleanArchitecture.Application.Features.Carts.Commands.UpdateCartItemQuantity;
using CleanArchitecture.Application.Features.Carts.Common;
using CleanArchitecture.Application.Features.Carts.Queries.GetCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("api/cart")]
public sealed class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{cartKey}")]
    public async Task<ActionResult<CartDto>> Get(string cartKey, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetCartQuery(cartKey), cancellationToken));
    }

    [HttpPost("{cartKey}/items")]
    public async Task<ActionResult<CartDto>> AddItem(
        string cartKey,
        [FromBody] AddCartItemCommand command,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command with { CartKey = cartKey }, cancellationToken));
    }

    [HttpPut("{cartKey}/items/{productId:int}")]
    public async Task<ActionResult<CartDto>> UpdateItem(
        string cartKey,
        int productId,
        [FromBody] UpdateCartItemQuantityCommand command,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command with { CartKey = cartKey, ProductId = productId }, cancellationToken));
    }

    [HttpDelete("{cartKey}/items/{productId:int}")]
    public async Task<ActionResult<CartDto>> RemoveItem(
        string cartKey,
        int productId,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new RemoveCartItemCommand(cartKey, productId), cancellationToken));
    }

    [HttpDelete("{cartKey}")]
    public async Task<IActionResult> Clear(string cartKey, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ClearCartCommand(cartKey), cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = "RequireCustomer")]
    [HttpPost("{cartKey}/merge")]
    public async Task<ActionResult<CartDto>> Merge(string cartKey, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new MergeCartCommand(cartKey), cancellationToken));
    }
}
