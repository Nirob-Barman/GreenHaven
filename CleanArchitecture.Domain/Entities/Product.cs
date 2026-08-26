using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities;

public sealed class Product : BaseEntity
{
    public string Title { get; private set; } = string.Empty;

    public string Slug { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public int QuantityInStock { get; private set; }

    public decimal Rating { get; private set; }

    public int CategoryId { get; private set; }

    public Category? Category { get; private set; }

    public string PrimaryImageUrl { get; private set; } = string.Empty;

    public string? PrimaryImagePublicId { get; private set; }

    public bool IsActive { get; private set; } = true;

    private Product()
    {
    }

    public static Product Create(
        int categoryId,
        string title,
        string slug,
        string description,
        decimal price,
        int quantityInStock,
        decimal rating,
        string primaryImageUrl,
        string? primaryImagePublicId)
    {
        var product = new Product();
        product.Update(
            categoryId,
            title,
            slug,
            description,
            price,
            quantityInStock,
            rating,
            primaryImageUrl,
            primaryImagePublicId);

        return product;
    }

    public void Update(
        int categoryId,
        string title,
        string slug,
        string description,
        decimal price,
        int quantityInStock,
        decimal rating,
        string primaryImageUrl,
        string? primaryImagePublicId)
    {
        if (price <= 0)
        {
            throw new DomainException("Product price must be greater than zero.");
        }

        if (quantityInStock < 0)
        {
            throw new DomainException("Product quantity cannot be negative.");
        }

        if (rating is < 0 or > 5)
        {
            throw new DomainException("Product rating must be between 0 and 5.");
        }

        CategoryId = categoryId;
        Title = title;
        Slug = slug;
        Description = description;
        Price = price;
        QuantityInStock = quantityInStock;
        Rating = rating;
        PrimaryImageUrl = primaryImageUrl;
        PrimaryImagePublicId = primaryImagePublicId;
        IsActive = true;
        MarkUpdated();
    }

    public void SetPrimaryImage(string primaryImageUrl, string? primaryImagePublicId)
    {
        if (string.IsNullOrWhiteSpace(primaryImageUrl))
        {
            throw new DomainException("Product image URL is required.");
        }

        PrimaryImageUrl = primaryImageUrl;
        PrimaryImagePublicId = primaryImagePublicId;
        MarkUpdated();
    }

    public void RemovePrimaryImage()
    {
        PrimaryImageUrl = string.Empty;
        PrimaryImagePublicId = null;
        MarkUpdated();
    }

    public void ChangeStock(int quantityInStock)
    {
        if (quantityInStock < 0)
        {
            throw new DomainException("Product quantity cannot be negative.");
        }

        QuantityInStock = quantityInStock;
        MarkUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkUpdated();
    }
}
