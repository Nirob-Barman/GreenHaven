using CleanArchitecture.Application.Features.Users.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery : IRequest<IReadOnlyCollection<AdminUserDto>>;
