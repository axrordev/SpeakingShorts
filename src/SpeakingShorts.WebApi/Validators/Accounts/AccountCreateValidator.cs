using FluentValidation;
using SpeakingShorts.WebApi.Helpers;

namespace SpeakingShorts.WebApi.Validators.Accounts;

public class AccountCreateValidator : AbstractValidator<string>
{
    public AccountCreateValidator()
    {
        RuleFor(model => model)
            .NotNull()
            .Must(ValidationHelper.IsValidEmail)
            .WithMessage("Email is not valid");
    }
}
