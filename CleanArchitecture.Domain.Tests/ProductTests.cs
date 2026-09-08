using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Tests;

public sealed class ProductTests
{
    [Fact]
    public void Create_WithValidValues_SetsProductState()
    {
        var product = Product.Create(1, "Fern", "fern", "Indoor fern", 12.50m, 8, 4.5m, "https://example.com/fern.jpg", "products/fern");

        Assert.Equal("Fern", product.Title);
        Assert.Equal(12.50m, product.Price);
        Assert.Equal(8, product.QuantityInStock);
        Assert.True(product.IsActive);
    }

    [Theory]
    [InlineData(0, 5, 4)]
    [InlineData(-1, 5, 4)]
    [InlineData(10, -1, 4)]
    [InlineData(10, 5, -1)]
    [InlineData(10, 5, 6)]
    public void Create_WithInvalidValues_ThrowsDomainException(decimal price, int stock, decimal rating)
    {
        Assert.Throws<DomainException>(() => Product.Create(
            1, "Fern", "fern", "Indoor fern", price, stock, rating, "https://example.com/fern.jpg", null));
    }

    [Fact]
    public void ChangeStock_WithNegativeValue_ThrowsDomainException()
    {
        var product = CreateProduct();

        Assert.Throws<DomainException>(() => product.ChangeStock(-1));
    }

    private static Product CreateProduct() => Product.Create(
        1, "Fern", "fern", "Indoor fern", 12.50m, 8, 4.5m, "https://example.com/fern.jpg", null);
}
