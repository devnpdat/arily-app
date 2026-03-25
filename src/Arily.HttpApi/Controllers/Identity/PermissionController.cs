using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.PermissionManagement;

namespace Arily.Controllers.Identity;

[ApiController]
[Route("api/app/permissions")]
[Authorize]
public class PermissionController : ArilyController
{
    private readonly IPermissionAppService _permissionAppService;
    private readonly ILogger<PermissionController> _logger;

    public PermissionController(IPermissionAppService permissionAppService, ILogger<PermissionController> logger)
    {
        _permissionAppService = permissionAppService;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách permissions theo provider.
    /// providerName: "R" (Role) | "U" (User) | "T" (Tenant)
    /// </summary>
    [HttpGet]
    public async Task<GetPermissionListResultDto> GetAsync([FromQuery] string providerName, [FromQuery] string providerKey)
    {
        _logger.LogInformation("GetPermissions: providerName={ProviderName} providerKey={ProviderKey}", providerName, providerKey);
        return await _permissionAppService.GetAsync(providerName, providerKey);
    }

    [HttpPut]
    public async Task UpdateAsync([FromQuery] string providerName, [FromQuery] string providerKey, [FromBody] UpdatePermissionsDto input)
    {
        _logger.LogInformation("UpdatePermissions: providerName={ProviderName} providerKey={ProviderKey}", providerName, providerKey);
        await _permissionAppService.UpdateAsync(providerName, providerKey, input);
    }
}
