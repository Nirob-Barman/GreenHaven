using FluentValidation;

namespace CleanArchitecture.Application.Features.Products.Commands.UpdateProductStock;

public sealed class UpdateProductStockCommandValidator : AbstractValidator<UpdateProductStockCommand>
{
    public UpdateProductStockCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0);
    }
}
