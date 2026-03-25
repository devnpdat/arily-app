using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Arily.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Arily.Controllers.Account;

[ApiController]
[Route("api/app/account")]
[Authorize]
public class AccountController : ArilyController
{
    private readonly IAccountAppService _accountAppService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountAppService accountAppService, ILogger<AccountController> logger)
    {
        _accountAppService = accountAppService;
        _logger = logger;
    }

    [HttpPut("change-email")]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailRequest input)
    {
        _logger.LogInformation("ChangeEmail request received");
        await _accountAppService.ChangeEmailAsync(new ChangeEmailInput { NewEmail = input.NewEmail });
        return Ok();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest input)
    {
        _logger.LogInformation("ChangePassword request received");
        await _accountAppService.ChangePasswordAsync(new ChangePasswordInput
        {
            CurrentPassword = input.CurrentPassword,
            NewPassword = input.NewPassword
        });
        return Ok();
    }

    [HttpPut("reset-password")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest input)
    {
        _logger.LogInformation("ResetPassword request received: targetUserId={UserId}", input.UserId);
        await _accountAppService.ResetPasswordAsync(new ResetPasswordInput
        {
            UserId = input.UserId,
            NewPassword = input.NewPassword
        });
        return Ok();
    }
}

public class ChangeEmailRequest
{
    [Required]
    [EmailAddress]
    public string NewEmail { get; set; } = null!;
}

public class ChangePasswordRequest
{
    [Required]
    public string CurrentPassword { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = null!;
}

public class ResetPasswordRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = null!;
}
