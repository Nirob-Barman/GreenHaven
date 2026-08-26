using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Products.Commands.DeleteProductImage;

public sealed class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IImageStorage _imageStorage;

    public DeleteProductImageCommandHandler(
        IApplicationDbContext context,
        IImageStorage imageStorage)
    {
        _context = context;
        _imageStorage = imageStorage;
    }

    public async Task<Unit> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException(nameof(Product), request.ProductId);
        }

        var publicId = product.PrimaryImagePublicId;
        if (!string.IsNullOrEmpty(publicId))
        {
            await _imageStorage.DeleteAsync(publicId, cancellationToken);
        }

        product.RemovePrimaryImage();
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
