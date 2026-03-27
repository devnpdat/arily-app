using System;
using System.Threading.Tasks;
using Arily.Collection.PurchaseOrders;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Collection;

[ApiController]
[Route("api/app/purchase-orders")]
[Authorize(ArilyPermissions.PurchaseOrders.Default)]
public class PurchaseOrderController : ArilyController
{
    private readonly IPurchaseOrderAppService _purchaseOrderAppService;
    private readonly ILogger<PurchaseOrderController> _logger;

    public PurchaseOrderController(
        IPurchaseOrderAppService purchaseOrderAppService,
        ILogger<PurchaseOrderController> logger)
    {
        _purchaseOrderAppService = purchaseOrderAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<PurchaseOrderDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetPurchaseOrder: id={Id}", id);
        return await _purchaseOrderAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<PurchaseOrderDto>> GetListAsync([FromQuery] GetPurchaseOrderListInput input)
    {
        _logger.LogInformation("GetPurchaseOrderList: sessionId={SessionId} farmerId={FarmerId}", input.SessionId, input.FarmerId);
        return await _purchaseOrderAppService.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.PurchaseOrders.Create)]
    public async Task<PurchaseOrderDto> CreateAsync([FromBody] CreatePurchaseOrderDto input)
    {
        _logger.LogInformation("CreatePurchaseOrder: sessionId={SessionId} farmerId={FarmerId} orderNo={OrderNo}",
            input.SessionId, input.FarmerId, input.OrderNo);
        return await _purchaseOrderAppService.CreateAsync(input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.PurchaseOrders.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeletePurchaseOrder: id={Id}", id);
        await _purchaseOrderAppService.DeleteAsync(id);
    }
}
