using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Auth.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.UpdateCurrentUser;

public sealed class UpdateCurrentUserCommandHandler : IRequestHandler<UpdateCurrentUserCommand, CurrentUserDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IIdentityService _identityService;

    public UpdateCurrentUserCommandHandler(
        ICurrentUser currentUser,
        IIdentityService identityService)
    {
        _currentUser = currentUser;
        _identityService = identityService;
    }

    public async Task<CurrentUserDto> Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
        {
            throw new ForbiddenAccessException();
        }

        var model = new UpdateProfileRequest(
            request.FullName,
            request.PhoneNumber,
            request.Address);

        return await _identityService.UpdateCurrentUserAsync(_currentUser.UserId, model, cancellationToken);
    }
}
