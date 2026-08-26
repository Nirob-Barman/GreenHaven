using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Products.Commands.UploadProductImage;

public sealed class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommand, ImageUploadResultDto>
{
    private const string Folder = "greenhaven/products";

    private readonly IApplicationDbContext _context;
    private readonly IImageStorage _imageStorage;
    private readonly ILogger<UploadProductImageCommandHandler> _logger;

    public UploadProductImageCommandHandler(
        IApplicationDbContext context,
        IImageStorage imageStorage,
        ILogger<UploadProductImageCommandHandler> logger)
    {
        _context = context;
        _imageStorage = imageStorage;
        _logger = logger;
    }

    public async Task<ImageUploadResultDto> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException(nameof(Product), request.ProductId);
        }

        var previousPublicId = product.PrimaryImagePublicId;

        var stored = await _imageStorage.UploadAsync(
            new ImageUploadRequest(request.Content, request.FileName, request.ContentType, Folder),
            cancellationToken);

        product.SetPrimaryImage(stored.Url, stored.PublicId);
        await _context.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrEmpty(previousPublicId) && previousPublicId != stored.PublicId)
        {
            try
            {
                await _imageStorage.DeleteAsync(previousPublicId, cancellationToken);
            }
            catch (ImageStorageException exception)
            {
                _logger.LogWarning(
                    exception,
                    "Failed to delete replaced image {PublicId} for product {ProductId}.",
                    previousPublicId,
                    product.Id);
            }
        }

        return new ImageUploadResultDto(stored.Url, stored.PublicId);
    }
}
