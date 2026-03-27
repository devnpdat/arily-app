using System;
using System.Threading.Tasks;
using Arily.Crm.FarmerGardens;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Crm;

[ApiController]
[Route("api/app/farmers/{farmerId:guid}/gardens")]
[Authorize(ArilyPermissions.FarmerGardens.Default)]
public class FarmerGardenController : ArilyController
{
    private readonly IFarmerGardenAppService _farmerGardenAppService;
    private readonly ILogger<FarmerGardenController> _logger;

    public FarmerGardenController(
        IFarmerGardenAppService farmerGardenAppService,
        ILogger<FarmerGardenController> logger)
    {
        _farmerGardenAppService = farmerGardenAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [Authorize(ArilyPermissions.FarmerGardens.Default)]
    public async Task<FarmerGardenDto> GetAsync(Guid farmerId, Guid id)
    {
        _logger.LogInformation("GetFarmerGarden: farmerId={FarmerId} id={Id}", farmerId, id);
        return await _farmerGardenAppService.GetAsync(farmerId, id);
    }

    [HttpGet]
    [Authorize(ArilyPermissions.FarmerGardens.Default)]
    public async Task<ListResultDto<FarmerGardenDto>> GetListAsync(Guid farmerId)
    {
        _logger.LogInformation("GetFarmerGardenList: farmerId={FarmerId}", farmerId);
        return await _farmerGardenAppService.GetListAsync(farmerId);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.FarmerGardens.Create)]
    public async Task<FarmerGardenDto> CreateAsync(Guid farmerId, [FromBody] CreateUpdateFarmerGardenDto input)
    {
        _logger.LogInformation("CreateFarmerGarden: farmerId={FarmerId} name={Name}", farmerId, input.GardenName);
        return await _farmerGardenAppService.CreateAsync(farmerId, input);
    }

    [HttpPut("{id:guid}")]
    [Authorize(ArilyPermissions.FarmerGardens.Edit)]
    public async Task<FarmerGardenDto> UpdateAsync(Guid farmerId, Guid id, [FromBody] CreateUpdateFarmerGardenDto input)
    {
        _logger.LogInformation("UpdateFarmerGarden: farmerId={FarmerId} id={Id}", farmerId, id);
        return await _farmerGardenAppService.UpdateAsync(farmerId, id, input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.FarmerGardens.Delete)]
    public async Task DeleteAsync(Guid farmerId, Guid id)
    {
        _logger.LogInformation("DeleteFarmerGarden: farmerId={FarmerId} id={Id}", farmerId, id);
        await _farmerGardenAppService.DeleteAsync(farmerId, id);
    }
}
