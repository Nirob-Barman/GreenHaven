using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Auth.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var model = new LoginRequest(request.Email, request.Password);
        return _identityService.LoginAsync(model, cancellationToken);
    }
}
