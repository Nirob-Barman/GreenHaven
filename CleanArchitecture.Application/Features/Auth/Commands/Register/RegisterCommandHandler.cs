using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Auth.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var model = new RegisterRequest(
            request.FullName,
            request.Email,
            request.Password,
            request.PhoneNumber,
            request.Address);

        return _identityService.RegisterAsync(model, cancellationToken);
    }
}
