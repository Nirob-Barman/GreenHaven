using CleanArchitecture.Application.Features.Auth.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.UpdateCurrentUser;

public sealed record UpdateCurrentUserCommand(
    string FullName,
    string? PhoneNumber,
    string? Address) : IRequest<CurrentUserDto>;
