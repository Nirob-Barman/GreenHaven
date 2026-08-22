using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(180);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.Price)
            .HasPrecision(18, 2);

        builder.Property(x => x.Rating)
            .HasPrecision(3, 2);

        builder.Property(x => x.PrimaryImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.PrimaryImagePublicId)
            .HasMaxLength(200);

        builder.Property(x => x.CategoryId)
            .IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.Slug).IsUnique();
        builder.HasIndex(x => x.CategoryId);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.Rating);
    }
}
