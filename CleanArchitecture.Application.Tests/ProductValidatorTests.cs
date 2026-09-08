using CleanArchitecture.Application.Features.Products.Commands.CreateProduct;

namespace CleanArchitecture.Application.Tests;

public sealed class ProductValidatorTests
{
    private readonly CreateProductCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidProduct_ReturnsSuccess()
    {
        var command = new CreateProductCommand(
            1, "Fern", "Indoor fern", 12.50m, 8, 4.5m, "https://example.com/fern.jpg", "products/fern");

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithInvalidPriceAndRating_ReturnsFailures()
    {
        var command = new CreateProductCommand(
            1, "Fern", "Indoor fern", 0m, 8, 6m, "https://example.com/fern.jpg", null);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateProductCommand.Price));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateProductCommand.Rating));
    }
}
