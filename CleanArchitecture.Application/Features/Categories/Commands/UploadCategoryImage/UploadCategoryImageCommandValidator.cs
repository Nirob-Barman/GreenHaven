using CleanArchitecture.Application.Common.Models;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Application.Features.Categories.Commands.UploadCategoryImage;

public sealed class UploadCategoryImageCommandValidator : AbstractValidator<UploadCategoryImageCommand>
{
    public UploadCategoryImageCommandValidator(IOptions<ImageUploadOptions> options)
    {
        var settings = options.Value;

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.FileSizeBytes)
            .GreaterThan(0)
            .WithMessage("An image file is required.")
            .LessThanOrEqualTo(settings.MaxFileSizeBytes)
            .WithMessage($"Image must not exceed {settings.MaxFileSizeBytes} bytes.");

        RuleFor(x => x.ContentType)
            .Must(contentType => settings.AllowedContentTypes.Contains(contentType))
            .WithMessage($"Image type must be one of: {string.Join(", ", settings.AllowedContentTypes)}.");
    }
}
