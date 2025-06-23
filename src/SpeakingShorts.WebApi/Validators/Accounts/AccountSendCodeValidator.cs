using FluentValidation;
using SpeakingShorts.WebApi.Helpers;

namespace SpeakingShorts.WebApi.Validators.Accounts;

public class AccountSendCodeValidator : AbstractValidator<string>
{
    public AccountSendCodeValidator()
    {
        RuleFor(model => model)
            .NotNull()
            .Must(ValidationHelper.IsValidEmail)
            .WithMessage("Email is not valid");
    }
}
