namespace CleanArchitecture.Application.Common.Interfaces;

public interface ICurrentUser
{
    string? UserId { get; }

    string? UserName { get; }

    string? Email { get; }

    bool IsAuthenticated { get; }
}
