using CleanArchitecture.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Application.Features.Categories.Commands.UploadCategoryImage;

public sealed record UploadCategoryImageCommand(
    int CategoryId,
    Stream Content,
    string FileName,
    string ContentType,
    long FileSizeBytes) : IRequest<ImageUploadResultDto>;
