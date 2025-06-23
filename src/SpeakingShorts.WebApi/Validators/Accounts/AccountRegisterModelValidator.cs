
using FluentValidation;
using SpeakingShorts.WebApi.Helpers;
using SpeakingShorts.WebApi.Models.Users;

namespace SpeakingShorts.WebApi.Validators.Accounts;

public class AccountRegisterModelValidator : AbstractValidator<UserRegisterModel>
{
    public AccountRegisterModelValidator()
    {
        RuleFor(user => user.Username)
            .NotNull()
            .NotEmpty()
            .WithMessage(user => $"{nameof(user.Username)} is not specified");


        RuleFor(user => user.Email)
            .NotNull()
            .Must(ValidationHelper.IsValidEmail)
            .WithMessage(user => $"{nameof(user.Email)} is not valid");

        RuleFor(user => user.Password)
            .NotNull()
            .Must(ValidationHelper.IsHardPassword)
            .WithMessage(user => $"{nameof(user.Password)} is not valid, password must be hard");
    }
}