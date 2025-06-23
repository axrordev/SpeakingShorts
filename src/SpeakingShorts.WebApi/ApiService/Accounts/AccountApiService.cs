using AutoMapper;
using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.Service.Helpers;
using SpeakingShorts.Service.Services.Accounts;
using SpeakingShorts.WebApi.Extensions;
using SpeakingShorts.WebApi.Models.Users;
using SpeakingShorts.WebApi.Validators.Accounts;

namespace SpeakingShorts.WebApi.ApiService.Accounts;

public class AccountApiService(
    IMapper mapper,
    IAccountService accountService,
    AccountLoginValidator loginValidator,
    AccountVerifyValidator verifyValidator,
    AccountCreateValidator createValidator,
    AccountSendCodeValidator sendCodeValidator,
    AccountRegisterModelValidator registerValidator,
    AccountResetPasswordValidator resetPasswordValidator) : IAccountApiService
{
    public async ValueTask RegisterAsync(UserRegisterModel registerModel)
    {
         await registerValidator.EnsureValidatedAsync(registerModel);

        var user = mapper.Map<User>(registerModel);
        user.PasswordHash = PasswordHasher.Hash(registerModel.Password);

        await accountService.RegisterAsync(user);
    }

    public async ValueTask RegisterVerifyAsync(string email, string code)
    {
        await verifyValidator.EnsureValidatedAsync(email, code);
        await accountService.RegisterVerifyAsync(email, code);

        await createValidator.EnsureValidatedAsync(email);
        await accountService.CreateAsync(email);
    }

    public async ValueTask<LoginViewModel> LoginAsync(string email, string password)
    {
        await loginValidator.EnsureValidatedAsync(email, password);
        var result = await accountService.LoginAsync(email, password);
        var mappedResult = mapper.Map<LoginViewModel>(result.user);
        mappedResult.Token = result.token;
        return mappedResult;
    }

    public async ValueTask<bool> SendCodeAsync(string email)
    {
        await sendCodeValidator.EnsureValidatedAsync(email);
        return await accountService.SendCodeAsync(email);
    }

    public async ValueTask<bool> VerifyAsync(string email, string code)
    {
        await verifyValidator.EnsureValidatedAsync(email, code);
        return await accountService.VerifyAsync(email, code);
    }

    public async ValueTask<UserViewModel> ResetPasswordAsync(string email, string newPassword)
    {
        await resetPasswordValidator.EnsureValidatedAsync(email, newPassword);
        var result = await accountService.ResetPasswordAsync(email, newPassword);
        return mapper.Map<UserViewModel>(result);
    }
}
