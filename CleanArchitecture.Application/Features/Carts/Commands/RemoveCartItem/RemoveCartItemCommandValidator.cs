using FluentValidation;

namespace CleanArchitecture.Application.Features.Carts.Commands.RemoveCartItem;

public sealed class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
{
    public RemoveCartItemCommandValidator()
    {
        RuleFor(x => x.CartKey)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.ProductId)
            .GreaterThan(0);
    }
}
