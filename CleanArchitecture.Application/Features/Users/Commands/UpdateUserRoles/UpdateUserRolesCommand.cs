using CleanArchitecture.Application.Features.Users.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Users.Commands.UpdateUserRoles;

public sealed record UpdateUserRolesCommand(
    string UserId,
    IReadOnlyCollection<string> Roles) : IRequest<AdminUserDto>;
