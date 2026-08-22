using FluentValidation;

namespace CleanArchitecture.Application.Features.Carts.Commands.ClearCart;

public sealed class ClearCartCommandValidator : AbstractValidator<ClearCartCommand>
{
    public ClearCartCommandValidator()
    {
        RuleFor(x => x.CartKey)
            .NotEmpty()
            .MaximumLength(64);
    }
}
