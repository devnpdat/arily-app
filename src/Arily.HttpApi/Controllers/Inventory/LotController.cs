using System;
using System.Threading.Tasks;
using Arily.Inventory.Lots;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Inventory;

[ApiController]
[Route("api/app/lots")]
[Authorize(ArilyPermissions.Lots.Default)]
public class LotController : ArilyController
{
    private readonly ILotAppService _lotAppService;
    private readonly ILogger<LotController> _logger;

    public LotController(
        ILotAppService lotAppService,
        ILogger<LotController> logger)
    {
        _lotAppService = lotAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<LotDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetLot: id={Id}", id);
        return await _lotAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<LotDto>> GetListAsync([FromQuery] GetLotListInput input)
    {
        _logger.LogInformation("GetLotList: warehouseId={WarehouseId} productId={ProductId} status={Status}",
            input.WarehouseId, input.ProductId, input.Status);
        return await _lotAppService.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.Lots.Create)]
    public async Task<LotDto> CreateAsync([FromBody] CreateLotDto input)
    {
        _logger.LogInformation("CreateLot: lotCode={LotCode} warehouseId={WarehouseId}", input.LotCode, input.WarehouseId);
        return await _lotAppService.CreateAsync(input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.Lots.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteLot: id={Id}", id);
        await _lotAppService.DeleteAsync(id);
    }
}
