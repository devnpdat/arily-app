using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Account;

namespace Arily.Controllers.Identity;

[ApiController]
[Route("api/app/account/profile")]
[Authorize]
public class ProfileController : ArilyController
{
    private readonly IProfileAppService _profileAppService;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(IProfileAppService profileAppService, ILogger<ProfileController> logger)
    {
        _profileAppService = profileAppService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ProfileDto> GetAsync()
    {
        _logger.LogInformation("GetProfile: userId={UserId}", CurrentUser.Id);
        return await _profileAppService.GetAsync();
    }

    [HttpPut]
    public async Task<ProfileDto> UpdateAsync([FromBody] UpdateProfileDto input)
    {
        _logger.LogInformation("UpdateProfile: userId={UserId}", CurrentUser.Id);
        return await _profileAppService.UpdateAsync(input);
    }

    [HttpPost("change-password")]
    public async Task ChangePasswordAsync([FromBody] ChangePasswordInput input)
    {
        _logger.LogInformation("ChangePassword via Profile: userId={UserId}", CurrentUser.Id);
        await _profileAppService.ChangePasswordAsync(input);
    }
}
