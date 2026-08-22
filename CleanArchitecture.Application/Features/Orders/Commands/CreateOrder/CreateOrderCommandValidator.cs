using CleanArchitecture.Domain.Enums;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CartKey)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.CustomerEmail)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(x => x.CustomerPhone)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.ShippingAddress)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Notes)
            .MaximumLength(1000);

        RuleFor(x => x.PaymentMethod)
            .Equal(PaymentMethod.CashOnDelivery);
    }
}
