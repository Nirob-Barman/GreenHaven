using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Features.Products.Commands.CreateProduct;
using CleanArchitecture.Application.Features.Products.Commands.DeleteProduct;
using CleanArchitecture.Application.Features.Products.Commands.UpdateProduct;
using CleanArchitecture.Application.Features.Products.Commands.UpdateProductStock;
using CleanArchitecture.Application.Features.Products.Common;
using CleanArchitecture.Application.Features.Products.Queries.GetProductById;
using CleanArchitecture.Application.Features.Products.Queries.GetProductBySlug;
using CleanArchitecture.Application.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProductListItemDto>>> GetAll(
        [FromQuery] GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDetailsDto>> GetById(int id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetProductByIdQuery(id), cancellationToken));
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ProductDetailsDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetProductBySlugQuery(slug), cancellationToken));
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpPost]
    public async Task<ActionResult<ProductDetailsDto>> Create(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDetailsDto>> Update(
        int id,
        [FromBody] UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command with { Id = id }, cancellationToken));
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpPatch("{id:int}/stock")]
    public async Task<ActionResult<int>> UpdateStock(
        int id,
        [FromBody] UpdateProductStockCommand command,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command with { Id = id }, cancellationToken));
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return NoContent();
    }
}
