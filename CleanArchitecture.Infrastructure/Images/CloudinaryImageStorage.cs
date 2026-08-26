using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Images;

public sealed class CloudinaryImageStorage : IImageStorage
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<CloudinaryImageStorage> _logger;

    public CloudinaryImageStorage(
        IOptions<CloudinaryOptions> options,
        ILogger<CloudinaryImageStorage> logger)
    {
        var settings = options.Value;

        if (string.IsNullOrWhiteSpace(settings.CloudName)
            || string.IsNullOrWhiteSpace(settings.ApiKey)
            || string.IsNullOrWhiteSpace(settings.ApiSecret))
        {
            throw new InvalidOperationException(
                "Cloudinary configuration is missing. Set Cloudinary:CloudName, Cloudinary:ApiKey and Cloudinary:ApiSecret.");
        }

        _cloudinary = new Cloudinary(new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret));
        _cloudinary.Api.Secure = true;
        _logger = logger;
    }

    public async Task<StoredImage> UploadAsync(ImageUploadRequest request, CancellationToken cancellationToken)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(request.FileName, request.Content),
            Folder = request.Folder,
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false
        };

        var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

        if (result.Error is not null || result.SecureUrl is null)
        {
            _logger.LogError(
                "Cloudinary upload failed with status {StatusCode}: {Error}",
                result.StatusCode,
                result.Error?.Message);

            throw new ImageStorageException("Image upload failed.");
        }

        return new StoredImage(result.SecureUrl.ToString(), result.PublicId);
    }

    public async Task DeleteAsync(string publicId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var deletionParams = new DeletionParams(publicId)
        {
            ResourceType = ResourceType.Image
        };

        var result = await _cloudinary.DestroyAsync(deletionParams);

        if (result.Error is not null)
        {
            _logger.LogError(
                "Cloudinary delete failed for {PublicId}: {Error}",
                publicId,
                result.Error.Message);

            throw new ImageStorageException("Image deletion failed.");
        }
    }
}
