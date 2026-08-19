namespace CleanArchitecture.Application.Common.Interfaces;

public interface IImageStorage
{
    Task<StoredImage> UploadAsync(
        ImageUploadRequest request,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        string publicId,
        CancellationToken cancellationToken);
}

public sealed record ImageUploadRequest(
    Stream Content,
    string FileName,
    string ContentType,
    string Folder);

public sealed record StoredImage(
    string Url,
    string PublicId);
