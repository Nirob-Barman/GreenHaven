using CleanArchitecture.Application.Features.Users.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(string UserId) : IRequest<AdminUserDto>;
