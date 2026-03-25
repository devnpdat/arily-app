using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.FeatureManagement;

namespace Arily.Controllers.Settings;

[ApiController]
[Route("api/app/features")]
[Authorize]
public class FeatureController : ArilyController
{
    private readonly IFeatureAppService _featureAppService;
    private readonly ILogger<FeatureController> _logger;

    public FeatureController(IFeatureAppService featureAppService, ILogger<FeatureController> logger)
    {
        _featureAppService = featureAppService;
        _logger = logger;
    }

    /// <summary>
    /// providerName: "T" (Tenant) | "E" (Edition)
    /// </summary>
    [HttpGet]
    public async Task<GetFeatureListResultDto> GetAsync([FromQuery] string providerName, [FromQuery] string? providerKey)
    {
        _logger.LogInformation("GetFeatures: providerName={ProviderName} providerKey={ProviderKey}", providerName, providerKey);
        return await _featureAppService.GetAsync(providerName, providerKey);
    }

    [HttpPut]
    public async Task UpdateAsync([FromQuery] string providerName, [FromQuery] string? providerKey, [FromBody] UpdateFeaturesDto input)
    {
        _logger.LogInformation("UpdateFeatures: providerName={ProviderName} providerKey={ProviderKey}", providerName, providerKey);
        await _featureAppService.UpdateAsync(providerName, providerKey, input);
    }

    [HttpDelete]
    [Authorize(Roles = "admin")]
    public async Task DeleteAsync([FromQuery] string providerName, [FromQuery] string? providerKey)
    {
        _logger.LogInformation("DeleteFeatures: providerName={ProviderName} providerKey={ProviderKey}", providerName, providerKey);
        await _featureAppService.DeleteAsync(providerName, providerKey);
    }
}
