using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities;

public sealed class Cart : BaseEntity
{
    public string CartKey { get; private set; } = string.Empty;

    public string? UserId { get; private set; }

    public ICollection<CartItem> Items { get; private set; } = new List<CartItem>();

    private Cart()
    {
    }

    public static Cart Create(string cartKey, string? userId)
    {
        return new Cart
        {
            CartKey = cartKey,
            UserId = userId
        };
    }

    public void AttachUser(string userId)
    {
        UserId = userId;
        MarkUpdated();
    }
}
