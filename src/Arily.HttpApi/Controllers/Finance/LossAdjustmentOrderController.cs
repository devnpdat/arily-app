using System;
using System.Threading.Tasks;
using Arily.Finance.LossAdjustmentOrders;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Finance;

[ApiController]
[Route("api/app/loss-adjustment-orders")]
[Authorize(ArilyPermissions.LossAdjustmentOrders.Default)]
public class LossAdjustmentOrderController : ArilyController
{
    private readonly ILossAdjustmentOrderAppService _appService;
    private readonly ILogger<LossAdjustmentOrderController> _logger;

    public LossAdjustmentOrderController(
        ILossAdjustmentOrderAppService appService,
        ILogger<LossAdjustmentOrderController> logger)
    {
        _appService = appService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [Authorize(ArilyPermissions.LossAdjustmentOrders.Default)]
    public async Task<LossAdjustmentOrderDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetLossAdjustmentOrder: id={Id}", id);
        return await _appService.GetAsync(id);
    }

    [HttpGet]
    [Authorize(ArilyPermissions.LossAdjustmentOrders.Default)]
    public async Task<PagedResultDto<LossAdjustmentOrderDto>> GetListAsync([FromQuery] GetLossAdjustmentOrderListInput input)
    {
        _logger.LogInformation("GetLossAdjustmentOrderList: farmerId={FarmerId} purchaseOrderId={PurchaseOrderId} status={Status}",
            input.FarmerId, input.PurchaseOrderId, input.Status);
        return await _appService.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.LossAdjustmentOrders.Create)]
    public async Task<LossAdjustmentOrderDto> CreateAsync([FromBody] CreateLossAdjustmentOrderDto input)
    {
        _logger.LogInformation("CreateLossAdjustmentOrder: adjustmentNo={AdjustmentNo} farmerId={FarmerId}",
            input.AdjustmentNo, input.FarmerId);
        return await _appService.CreateAsync(input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.LossAdjustmentOrders.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteLossAdjustmentOrder: id={Id}", id);
        await _appService.DeleteAsync(id);
    }
}
