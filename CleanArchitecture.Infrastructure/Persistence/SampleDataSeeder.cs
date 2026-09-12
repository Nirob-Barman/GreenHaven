using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure.Persistence;

public static class SampleDataSeeder
{
    private static readonly (string Name, string Slug, string Description)[] Categories =
    [
        ("Indoor Plants", "indoor-plants", "Low-maintenance plants for bright indoor spaces."),
        ("Outdoor Plants", "outdoor-plants", "Plants suited to gardens, balconies, and outdoor spaces."),
        ("Flowering Plants", "flowering-plants", "Colorful flowering plants for home and garden."),
        ("Succulents", "succulents", "Compact, drought-tolerant plants with distinctive forms."),
        ("Seeds", "seeds", "Seeds for growing herbs, flowers, and vegetables."),
        ("Gardening Tools", "gardening-tools", "Useful tools for planting and everyday garden care.")
    ];

    private static readonly (string CategorySlug, string Title, string Slug, string Description, decimal Price, int Stock, decimal Rating, string ImageUrl)[] Products =
    [
        ("indoor-plants", "Peace Lily", "peace-lily", "An elegant indoor plant with glossy leaves and white blooms.", 18.00m, 20, 4.7m, "https://images.unsplash.com/photo-1593691509543-c55fb32e5cee"),
        ("indoor-plants", "Snake Plant", "snake-plant", "A resilient plant that adds structure to indoor spaces.", 22.50m, 15, 4.8m, "https://images.unsplash.com/photo-1593482892290-f54927ae2bb6"),
        ("outdoor-plants", "Areca Palm", "areca-palm", "A graceful palm for bright patios and outdoor corners.", 35.00m, 10, 4.5m, "https://images.unsplash.com/photo-1545239351-1141bd82e8a6"),
        ("flowering-plants", "Rose Plant", "rose-plant", "A flowering rose plant for a colorful garden display.", 25.00m, 12, 4.6m, "https://images.unsplash.com/photo-1496062031456-07b8f162a322"),
        ("succulents", "Aloe Vera", "aloe-vera", "A hardy succulent with useful, fleshy leaves.", 14.00m, 24, 4.7m, "https://images.unsplash.com/photo-1558642452-9d2a7deb7f62"),
        ("seeds", "Basil Seeds", "basil-seeds", "A packet of seeds for growing fresh basil at home.", 4.50m, 50, 4.4m, "https://images.unsplash.com/photo-1618375569909-3c8616cf7733"),
        ("gardening-tools", "Hand Trowel", "hand-trowel", "A compact stainless-steel trowel for planting and potting.", 9.00m, 30, 4.3m, "https://images.unsplash.com/photo-1416879595882-3373a0480b5b")
    ];

    public static async Task SeedAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        foreach (var definition in Categories)
        {
            if (await context.Categories.AnyAsync(x => x.Slug == definition.Slug, cancellationToken))
            {
                continue;
            }

            context.Categories.Add(Category.Create(
                definition.Name,
                definition.Slug,
                definition.Description,
                null,
                null));
        }

        await context.SaveChangesAsync(cancellationToken);

        var categoriesBySlug = await context.Categories
            .Where(x => Categories.Select(category => category.Slug).Contains(x.Slug))
            .ToDictionaryAsync(x => x.Slug, cancellationToken);

        foreach (var definition in Products)
        {
            if (await context.Products.AnyAsync(x => x.Slug == definition.Slug, cancellationToken))
            {
                continue;
            }

            if (!categoriesBySlug.TryGetValue(definition.CategorySlug, out var category))
            {
                continue;
            }

            context.Products.Add(Product.Create(
                category.Id,
                definition.Title,
                definition.Slug,
                definition.Description,
                definition.Price,
                definition.Stock,
                definition.Rating,
                definition.ImageUrl,
                null));
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
