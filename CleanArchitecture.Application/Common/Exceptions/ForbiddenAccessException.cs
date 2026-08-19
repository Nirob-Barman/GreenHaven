namespace CleanArchitecture.Application.Common.Exceptions;

public sealed class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException()
        : base("Forbidden access.")
    {
    }
}
