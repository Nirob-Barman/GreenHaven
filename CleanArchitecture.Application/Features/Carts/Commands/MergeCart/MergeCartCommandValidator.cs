using FluentValidation;

namespace CleanArchitecture.Application.Features.Carts.Commands.MergeCart;

public sealed class MergeCartCommandValidator : AbstractValidator<MergeCartCommand>
{
    public MergeCartCommandValidator()
    {
        RuleFor(x => x.CartKey)
            .NotEmpty()
            .MaximumLength(64);
    }
}
