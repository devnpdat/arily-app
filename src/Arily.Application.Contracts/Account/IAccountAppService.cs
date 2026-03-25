using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Arily.Account;

public interface IAccountAppService : IApplicationService
{
    Task ChangeEmailAsync(ChangeEmailInput input);
    Task ChangePasswordAsync(ChangePasswordInput input);
    Task ResetPasswordAsync(ResetPasswordInput input);
}

public class ChangeEmailInput
{
    public string NewEmail { get; set; } = null!;
}

public class ChangePasswordInput
{
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}

public class ResetPasswordInput
{
    public Guid UserId { get; set; }
    public string NewPassword { get; set; } = null!;
}
