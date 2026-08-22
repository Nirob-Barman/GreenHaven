using FluentValidation;

namespace CleanArchitecture.Application.Features.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

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
