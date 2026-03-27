using System;
using System.Threading.Tasks;
using Arily.Inventory.InventoryLots;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Inventory;

[ApiController]
[Route("api/app/inventory-lots")]
[Authorize(ArilyPermissions.Lots.Default)]
public class InventoryLotController : ArilyController
{
    private readonly IInventoryLotAppService _inventoryLotAppService;
    private readonly ILogger<InventoryLotController> _logger;

    public InventoryLotController(
        IInventoryLotAppService inventoryLotAppService,
        ILogger<InventoryLotController> logger)
    {
        _inventoryLotAppService = inventoryLotAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<InventoryLotDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetInventoryLot: id={Id}", id);
        return await _inventoryLotAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<InventoryLotDto>> GetListAsync([FromQuery] GetInventoryLotListInput input)
    {
        _logger.LogInformation("GetInventoryLotList: warehouseId={WarehouseId} lotId={LotId} productId={ProductId}",
            input.WarehouseId, input.LotId, input.ProductId);
        return await _inventoryLotAppService.GetListAsync(input);
    }
}
