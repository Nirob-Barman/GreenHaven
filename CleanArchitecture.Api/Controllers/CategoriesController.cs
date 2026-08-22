using CleanArchitecture.Application.Features.Categories.Commands.CreateCategory;
using CleanArchitecture.Application.Features.Categories.Commands.DeleteCategory;
using CleanArchitecture.Application.Features.Categories.Commands.UpdateCategory;
using CleanArchitecture.Application.Features.Categories.Common;
using CleanArchitecture.Application.Features.Categories.Queries.GetCategories;
using CleanArchitecture.Application.Features.Categories.Queries.GetCategoryById;
using CleanArchitecture.Application.Features.Categories.Queries.GetCategoryBySlug;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<CategoryDto>>> GetAll(
        [FromQuery] bool includeInactive,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetCategoriesQuery(includeInactive), cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken));
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<CategoryDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetCategoryBySlugQuery(slug), cancellationToken));
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(
        [FromBody] CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDto>> Update(
        int id,
        [FromBody] UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command with { Id = id }, cancellationToken));
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
        return NoContent();
    }
}
