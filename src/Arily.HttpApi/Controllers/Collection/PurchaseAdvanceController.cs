using System;
using System.Threading.Tasks;
using Arily.Collection.PurchaseAdvances;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Collection;

[ApiController]
[Route("api/app/purchase-advances")]
[Authorize(ArilyPermissions.PurchaseAdvances.Default)]
public class PurchaseAdvanceController : ArilyController
{
    private readonly IPurchaseAdvanceAppService _purchaseAdvanceAppService;
    private readonly ILogger<PurchaseAdvanceController> _logger;

    public PurchaseAdvanceController(
        IPurchaseAdvanceAppService purchaseAdvanceAppService,
        ILogger<PurchaseAdvanceController> logger)
    {
        _purchaseAdvanceAppService = purchaseAdvanceAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<PurchaseAdvanceDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetPurchaseAdvance: id={Id}", id);
        return await _purchaseAdvanceAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<ListResultDto<PurchaseAdvanceDto>> GetListAsync([FromQuery] Guid purchaseOrderId)
    {
        _logger.LogInformation("GetPurchaseAdvanceList: purchaseOrderId={PurchaseOrderId}", purchaseOrderId);
        return await _purchaseAdvanceAppService.GetListAsync(purchaseOrderId);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.PurchaseAdvances.Create)]
    public async Task<PurchaseAdvanceDto> CreateAsync([FromBody] CreatePurchaseAdvanceDto input)
    {
        _logger.LogInformation("CreatePurchaseAdvance: purchaseOrderId={PurchaseOrderId} farmerId={FarmerId} amount={Amount}",
            input.PurchaseOrderId, input.FarmerId, input.Amount);
        return await _purchaseAdvanceAppService.CreateAsync(input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.PurchaseAdvances.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeletePurchaseAdvance: id={Id}", id);
        await _purchaseAdvanceAppService.DeleteAsync(id);
    }
}
