using FluentValidation;

namespace CleanArchitecture.Application.Features.Carts.Queries.GetCart;

public sealed class GetCartQueryValidator : AbstractValidator<GetCartQuery>
{
    public GetCartQueryValidator()
    {
        RuleFor(x => x.CartKey)
            .NotEmpty()
            .MaximumLength(64);
    }
}
