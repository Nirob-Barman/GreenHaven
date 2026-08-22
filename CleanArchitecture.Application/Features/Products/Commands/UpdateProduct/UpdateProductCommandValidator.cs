using FluentValidation;

namespace CleanArchitecture.Application.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Rating)
            .InclusiveBetween(0, 5);

        RuleFor(x => x.PrimaryImageUrl)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.PrimaryImagePublicId)
            .MaximumLength(200);
    }
}
