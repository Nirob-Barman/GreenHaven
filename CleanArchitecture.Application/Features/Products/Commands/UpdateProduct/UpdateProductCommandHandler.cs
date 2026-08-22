using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Products.Common;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDetailsDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDetailsDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId && x.IsActive, cancellationToken);
        if (category is null)
        {
            throw new NotFoundException(nameof(Category), request.CategoryId);
        }

        var slug = Slugify(request.Title);
        var duplicate = await _context.Products.AnyAsync(x => x.Id != request.Id && (x.Title == request.Title || x.Slug == slug), cancellationToken);
        if (duplicate)
        {
            throw new InvalidOperationException("Product title or slug already exists.");
        }

        product.Update(
            request.CategoryId,
            request.Title,
            slug,
            request.Description,
            request.Price,
            request.QuantityInStock,
            request.Rating,
            request.PrimaryImageUrl,
            request.PrimaryImagePublicId);

        if (!request.IsActive)
        {
            product.Deactivate();
        }

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
