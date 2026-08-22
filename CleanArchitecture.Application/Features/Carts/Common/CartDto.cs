namespace CleanArchitecture.Application.Features.Carts.Common;

public sealed record CartDto(
    string CartKey,
    IReadOnlyCollection<CartItemDto> Items,
    decimal Subtotal,
    int TotalItems);
