using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public string Slug { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? ImageUrl { get; private set; }

    public string? ImagePublicId { get; private set; }

    public bool IsActive { get; private set; } = true;

    public ICollection<Product> Products { get; private set; } = new List<Product>();

    private Category()
    {
    }

    public static Category Create(
        string name,
        string slug,
        string? description,
        string? imageUrl,
        string? imagePublicId)
    {
        var category = new Category();
        category.Update(name, slug, description, imageUrl, imagePublicId);
        return category;
    }

    public void Update(
        string name,
        string slug,
        string? description,
        string? imageUrl,
        string? imagePublicId)
    {
        Name = name;
        Slug = slug;
        Description = description;
        ImageUrl = imageUrl;
        ImagePublicId = imagePublicId;
        IsActive = true;
        MarkUpdated();
    }

    public void SetImage(string imageUrl, string? imagePublicId)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            throw new DomainException("Category image URL is required.");
        }

        ImageUrl = imageUrl;
        ImagePublicId = imagePublicId;
        MarkUpdated();
    }

    public void RemoveImage()
    {
        ImageUrl = null;
        ImagePublicId = null;
        MarkUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkUpdated();
    }
}
