using CleanArchitecture.Application.Features.Auth.Commands.Login;
using CleanArchitecture.Application.Features.Auth.Commands.Logout;
using CleanArchitecture.Application.Features.Auth.Commands.RefreshToken;
using CleanArchitecture.Application.Features.Auth.Commands.Register;
using CleanArchitecture.Application.Features.Auth.Commands.UpdateCurrentUser;
using CleanArchitecture.Application.Features.Auth.Common;
using CleanArchitecture.Application.Features.Auth.Queries.GetCurrentUser;
using CleanArchitecture.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly JwtOptions _jwtOptions;

    public AuthController(IMediator mediator, IOptions<JwtOptions> jwtOptions)
    {
        _mediator = mediator;
        _jwtOptions = jwtOptions.Value;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        [FromBody] RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        SetRefreshTokenCookie(result.RefreshToken);
        return Ok(result.Response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        SetRefreshTokenCookie(result.RefreshToken);
        return Ok(result.Response);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponse>> RefreshToken(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue(_jwtOptions.RefreshTokenCookieName, out var refreshToken) || string.IsNullOrWhiteSpace(refreshToken))
        {
            return Unauthorized();
        }

        var result = await _mediator.Send(new RefreshTokenCommand(refreshToken), cancellationToken);
        SetRefreshTokenCookie(result.RefreshToken);
        return Ok(result.Response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        if (Request.Cookies.TryGetValue(_jwtOptions.RefreshTokenCookieName, out var refreshToken) && !string.IsNullOrWhiteSpace(refreshToken))
        {
            await _mediator.Send(new LogoutCommand(refreshToken), cancellationToken);
        }

        Response.Cookies.Delete(_jwtOptions.RefreshTokenCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/api/auth"
        });

        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserDto>> Me(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCurrentUserQuery(), cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<ActionResult<CurrentUserDto>> UpdateMe(
        [FromBody] UpdateCurrentUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        Response.Cookies.Append(_jwtOptions.RefreshTokenCookieName, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/api/auth",
            Expires = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiresDays)
        });
    }
}
