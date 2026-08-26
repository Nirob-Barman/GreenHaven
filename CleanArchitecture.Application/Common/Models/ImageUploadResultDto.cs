namespace CleanArchitecture.Application.Common.Models;

public sealed record ImageUploadResultDto(
    string Url,
    string? PublicId);
