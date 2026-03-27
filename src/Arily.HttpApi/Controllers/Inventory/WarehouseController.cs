using System;
using System.Threading.Tasks;
using Arily.Inventory.Warehouses;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Inventory;

[ApiController]
[Route("api/app/warehouses")]
[Authorize(ArilyPermissions.Warehouses.Default)]
public class WarehouseController : ArilyController
{
    private readonly IWarehouseAppService _warehouseAppService;
    private readonly ILogger<WarehouseController> _logger;

    public WarehouseController(
        IWarehouseAppService warehouseAppService,
        ILogger<WarehouseController> logger)
    {
        _warehouseAppService = warehouseAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<WarehouseDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetWarehouse: id={Id}", id);
        return await _warehouseAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<WarehouseDto>> GetListAsync([FromQuery] GetWarehouseListInput input)
    {
        _logger.LogInformation("GetWarehouseList: filter={Filter} status={Status}", input.Filter, input.Status);
        return await _warehouseAppService.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.Warehouses.Create)]
    public async Task<WarehouseDto> CreateAsync([FromBody] CreateUpdateWarehouseDto input)
    {
        _logger.LogInformation("CreateWarehouse: code={Code} name={Name}", input.Code, input.Name);
        return await _warehouseAppService.CreateAsync(input);
    }

    [HttpPut("{id:guid}")]
    [Authorize(ArilyPermissions.Warehouses.Edit)]
    public async Task<WarehouseDto> UpdateAsync(Guid id, [FromBody] CreateUpdateWarehouseDto input)
    {
        _logger.LogInformation("UpdateWarehouse: id={Id}", id);
        return await _warehouseAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.Warehouses.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteWarehouse: id={Id}", id);
        await _warehouseAppService.DeleteAsync(id);
    }
}
