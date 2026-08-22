using FluentValidation;

namespace CleanArchitecture.Application.Features.Carts.Commands.UpdateCartItemQuantity;

public sealed class UpdateCartItemQuantityCommandValidator : AbstractValidator<UpdateCartItemQuantityCommand>
{
    public UpdateCartItemQuantityCommandValidator()
    {
        RuleFor(x => x.CartKey)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.ProductId)
            .GreaterThan(0);

        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}
