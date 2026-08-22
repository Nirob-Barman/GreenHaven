using FluentValidation;

namespace CleanArchitecture.Application.Features.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500);

        RuleFor(x => x.ImagePublicId)
            .MaximumLength(200);
    }
}
