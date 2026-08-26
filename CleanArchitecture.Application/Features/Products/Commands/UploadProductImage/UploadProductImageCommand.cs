using CleanArchitecture.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Application.Features.Products.Commands.UploadProductImage;

public sealed record UploadProductImageCommand(
    int ProductId,
    Stream Content,
    string FileName,
    string ContentType,
    long FileSizeBytes) : IRequest<ImageUploadResultDto>;
