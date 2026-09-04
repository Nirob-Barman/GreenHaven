using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Features.Users.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Users.Commands.UpdateUserRoles;

public sealed class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, AdminUserDto>
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentUser _currentUser;

    public UpdateUserRolesCommandHandler(
        IIdentityService identityService,
        ICurrentUser currentUser)
    {
        _identityService = identityService;
        _currentUser = currentUser;
    }

    public Task<AdminUserDto> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        if (string.Equals(_currentUser.UserId, request.UserId, StringComparison.OrdinalIgnoreCase)
            && !request.Roles.Contains("Admin", StringComparer.OrdinalIgnoreCase))
        {
            throw new ForbiddenAccessException();
        }

        return _identityService.UpdateUserRolesAsync(request.UserId, request.Roles, cancellationToken);
    }
}
