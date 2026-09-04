using CleanArchitecture.Api.Contracts.Users;
using CleanArchitecture.Application.Features.Users.Commands.UpdateUserRoles;
using CleanArchitecture.Application.Features.Users.Common;
using CleanArchitecture.Application.Features.Users.Queries.GetUserById;
using CleanArchitecture.Application.Features.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Authorize(Policy = "RequireAdmin")]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<AdminUserDto>>> GetAll(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetUsersQuery(), cancellationToken));

    [HttpGet("{userId}")]
    public async Task<ActionResult<AdminUserDto>> GetById(string userId, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetUserByIdQuery(userId), cancellationToken));

    [HttpPut("{userId}/roles")]
    public async Task<ActionResult<AdminUserDto>> UpdateRoles(
        string userId,
        [FromBody] UpdateUserRolesRequest request,
        CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new UpdateUserRolesCommand(userId, request.Roles), cancellationToken));
}
