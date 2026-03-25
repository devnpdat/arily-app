using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.SettingManagement;

namespace Arily.Controllers.Settings;

[ApiController]
[Route("api/app/settings/email")]
[Authorize(Roles = "admin")]
public class EmailSettingsController : ArilyController
{
    private readonly IEmailSettingsAppService _emailSettingAppService;
    private readonly ILogger<EmailSettingsController> _logger;

    public EmailSettingsController(IEmailSettingsAppService emailSettingAppService, ILogger<EmailSettingsController> logger)
    {
        _emailSettingAppService = emailSettingAppService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<EmailSettingsDto> GetAsync()
    {
        _logger.LogInformation("GetEmailSettings");
        return await _emailSettingAppService.GetAsync();
    }

    [HttpPut]
    public async Task UpdateAsync([FromBody] UpdateEmailSettingsDto input)
    {
        _logger.LogInformation("UpdateEmailSettings: host={Host} port={Port} sender={Sender}", input.SmtpHost, input.SmtpPort, input.DefaultFromAddress);
        await _emailSettingAppService.UpdateAsync(input);
    }
}
