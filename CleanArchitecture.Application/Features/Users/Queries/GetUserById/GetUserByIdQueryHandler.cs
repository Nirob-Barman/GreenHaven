using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Users.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, AdminUserDto>
{
    private readonly IIdentityService _identityService;

    public GetUserByIdQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<AdminUserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        => _identityService.GetUserByIdAsync(request.UserId, cancellationToken);
}
