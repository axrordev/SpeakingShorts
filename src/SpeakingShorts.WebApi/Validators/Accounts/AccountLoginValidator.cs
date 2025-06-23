using FluentValidation;
using SpeakingShorts.WebApi.Helpers;

namespace SpeakingShorts.WebApi.Validators.Accounts;

public class AccountLoginValidator : AbstractValidator<(string email, string password)>
{
    public AccountLoginValidator()
    {
        RuleFor(model => model.email)
            .NotNull()
            .Must(ValidationHelper.IsValidEmail)
            .WithMessage("Email is not valid");

        RuleFor(model => model.password)
            .NotNull()
            .Must(ValidationHelper.IsHardPassword)
            .WithMessage("Password is not valid, password must be hard!");
    }
}