using CleanArchitecture.Application.Features.Auth.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<AuthResult>;
