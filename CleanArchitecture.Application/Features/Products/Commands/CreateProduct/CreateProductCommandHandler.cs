using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Products.Common;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDetailsDto>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDetailsDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId && x.IsActive, cancellationToken);
        if (category is null)
        {
            throw new NotFoundException(nameof(Category), request.CategoryId);
        }

        var slug = Slugify(request.Title);
        var exists = await _context.Products.AnyAsync(x => x.Title == request.Title || x.Slug == slug, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException("Product title or slug already exists.");
        }

        var product = Product.Create(
            request.CategoryId,
            request.Title,
            slug,
            request.Description,
            request.Price,
            request.QuantityInStock,
            request.Rating,
            request.PrimaryImageUrl,
            request.PrimaryImagePublicId);

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return new ProductDetailsDto(
            product.Id,
            product.Title,
            product.Slug,
            product.Description,
            product.Price,
            product.QuantityInStock,
            product.Rating,
            product.CategoryId,
            category.Name,
            product.PrimaryImageUrl,
            product.IsActive);
    }

    private static string Slugify(string value)
    {
        return value.Trim().ToLowerInvariant().Replace(" ", "-");
    }
}
