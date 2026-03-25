using System;
using System.Threading.Tasks;
using Arily.Crm.Farmers;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Crm;

[ApiController]
[Route("api/app/farmers")]
[Authorize(ArilyPermissions.Farmers.Default)]
public class FarmerController : ArilyController
{
    private readonly IFarmerAppService _farmerAppService;
    private readonly ILogger<FarmerController> _logger;

    public FarmerController(IFarmerAppService farmerAppService, ILogger<FarmerController> logger)
    {
        _farmerAppService = farmerAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [Authorize(ArilyPermissions.Farmers.Default)]
    public async Task<FarmerDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetFarmer: id={Id}", id);
        return await _farmerAppService.GetAsync(id);
    }

    [HttpGet]
    [Authorize(ArilyPermissions.Farmers.Default)]
    public async Task<PagedResultDto<FarmerDto>> GetListAsync([FromQuery] GetFarmerListInput input)
    {
        _logger.LogInformation("GetFarmerList: filter={Filter} status={Status} province={Province} skip={Skip} max={Max}",
            input.Filter, input.Status, input.ProvinceCode, input.SkipCount, input.MaxResultCount);
        return await _farmerAppService.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.Farmers.Create)]
    public async Task<FarmerDto> CreateAsync([FromBody] CreateUpdateFarmerDto input)
    {
        _logger.LogInformation("CreateFarmer: code={Code} name={FullName}", input.Code, input.FullName);
        return await _farmerAppService.CreateAsync(input);
    }

    [HttpPut("{id:guid}")]
    [Authorize(ArilyPermissions.Farmers.Edit)]
    public async Task<FarmerDto> UpdateAsync(Guid id, [FromBody] CreateUpdateFarmerDto input)
    {
        _logger.LogInformation("UpdateFarmer: id={Id} code={Code} name={FullName}", id, input.Code, input.FullName);
        return await _farmerAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.Farmers.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteFarmer: id={Id}", id);
        await _farmerAppService.DeleteAsync(id);
    }
}
