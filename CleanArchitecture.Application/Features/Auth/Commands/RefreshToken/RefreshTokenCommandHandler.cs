using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Auth.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResult>
{
    private readonly IIdentityService _identityService;

    public RefreshTokenCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return _identityService.RefreshTokenAsync(request.RefreshToken, cancellationToken);
    }
}
