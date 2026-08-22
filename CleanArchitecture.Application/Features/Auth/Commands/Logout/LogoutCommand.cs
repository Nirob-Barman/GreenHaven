using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.Logout;

public sealed record LogoutCommand(string RefreshToken) : IRequest<Unit>;
