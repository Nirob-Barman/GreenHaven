using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Categories.Commands.UploadCategoryImage;

public sealed class UploadCategoryImageCommandHandler : IRequestHandler<UploadCategoryImageCommand, ImageUploadResultDto>
{
    private const string Folder = "greenhaven/categories";

    private readonly IApplicationDbContext _context;
    private readonly IImageStorage _imageStorage;
    private readonly ILogger<UploadCategoryImageCommandHandler> _logger;

    public UploadCategoryImageCommandHandler(
        IApplicationDbContext context,
        IImageStorage imageStorage,
        ILogger<UploadCategoryImageCommandHandler> logger)
    {
        _context = context;
        _imageStorage = imageStorage;
        _logger = logger;
    }

    public async Task<ImageUploadResultDto> Handle(UploadCategoryImageCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new NotFoundException(nameof(Category), request.CategoryId);
        }

        var previousPublicId = category.ImagePublicId;

        var stored = await _imageStorage.UploadAsync(
            new ImageUploadRequest(request.Content, request.FileName, request.ContentType, Folder),
            cancellationToken);

        category.SetImage(stored.Url, stored.PublicId);
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
                    "Failed to delete replaced image {PublicId} for category {CategoryId}.",
                    previousPublicId,
                    category.Id);
            }
        }

        return new ImageUploadResultDto(stored.Url, stored.PublicId);
    }
}
