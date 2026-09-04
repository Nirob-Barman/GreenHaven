using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Users.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyCollection<AdminUserDto>>
{
    private readonly IIdentityService _identityService;

    public GetUsersQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<IReadOnlyCollection<AdminUserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        => _identityService.GetUsersAsync(cancellationToken);
}
