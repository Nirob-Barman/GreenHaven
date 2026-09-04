using FluentValidation;

namespace CleanArchitecture.Application.Features.Users.Commands.UpdateUserRoles;

public sealed class UpdateUserRolesCommandValidator : AbstractValidator<UpdateUserRolesCommand>
{
    private static readonly string[] AllowedRoles = ["Admin", "Customer"];

    public UpdateUserRolesCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Roles)
            .NotEmpty()
            .Must(roles => roles.Distinct(StringComparer.OrdinalIgnoreCase).Count() == roles.Count)
            .WithMessage("Roles must not contain duplicates.")
            .Must(roles => roles.All(role => AllowedRoles.Contains(role, StringComparer.OrdinalIgnoreCase)))
            .WithMessage("Only Admin and Customer roles are supported.");
    }
}
