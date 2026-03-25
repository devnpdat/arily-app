using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Identity;

namespace Arily.Account;

[Authorize]
public class AccountAppService : ArilyAppService, IAccountAppService
{
    private readonly IdentityUserManager _userManager;

    public AccountAppService(IdentityUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task ChangeEmailAsync(ChangeEmailInput input)
    {
        Logger.LogInformation("ChangeEmail: userId={UserId} newEmail={Email}", CurrentUser.Id, input.NewEmail);

        var user = await _userManager.GetByIdAsync(CurrentUser.Id!.Value);

        var token = await _userManager.GenerateChangeEmailTokenAsync(user, input.NewEmail);
        var result = await _userManager.ChangeEmailAsync(user, input.NewEmail, token);

        if (!result.Succeeded)
        {
            Logger.LogWarning("ChangeEmail failed: userId={UserId} errors={Errors}", CurrentUser.Id, result.Errors);
            throw new UserFriendlyException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await _userManager.SetUserNameAsync(user, input.NewEmail);

        Logger.LogInformation("ChangeEmail succeeded: userId={UserId}", CurrentUser.Id);
    }

    public async Task ChangePasswordAsync(ChangePasswordInput input)
    {
        Logger.LogInformation("ChangePassword: userId={UserId}", CurrentUser.Id);

        var user = await _userManager.GetByIdAsync(CurrentUser.Id!.Value);

        var result = await _userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);

        if (!result.Succeeded)
        {
            Logger.LogWarning("ChangePassword failed: userId={UserId} errors={Errors}", CurrentUser.Id, result.Errors);
            throw new UserFriendlyException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        Logger.LogInformation("ChangePassword succeeded: userId={UserId}", CurrentUser.Id);
    }

    [Authorize(Roles = "admin")]
    public async Task ResetPasswordAsync(ResetPasswordInput input)
    {
        Logger.LogInformation("ResetPassword: targetUserId={UserId}", input.UserId);

        var user = await _userManager.GetByIdAsync(input.UserId);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, input.NewPassword);

        if (!result.Succeeded)
        {
            Logger.LogWarning("ResetPassword failed: targetUserId={UserId} errors={Errors}", input.UserId, result.Errors);
            throw new UserFriendlyException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        Logger.LogInformation("ResetPassword succeeded: targetUserId={UserId}", input.UserId);
    }
}
