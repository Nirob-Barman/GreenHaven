using CleanArchitecture.Application.Features.Auth.Common;
using MediatR;

namespace CleanArchitecture.Application.Features.Auth.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery : IRequest<CurrentUserDto>;
