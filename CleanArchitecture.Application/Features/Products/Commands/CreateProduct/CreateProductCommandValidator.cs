using FluentValidation;

namespace CleanArchitecture.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
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
