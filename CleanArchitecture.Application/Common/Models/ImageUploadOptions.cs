namespace CleanArchitecture.Application.Common.Models;

public sealed class ImageUploadOptions
{
    public const string SectionName = "ImageUpload";

    public long MaxFileSizeBytes { get; set; } = 2 * 1024 * 1024;

    public string[] AllowedContentTypes { get; set; } =
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };
}
