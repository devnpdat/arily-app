using System;
using System.Threading.Tasks;
using Arily.Permissions;
using Arily.Sales.SalesOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Sales;

[ApiController]
[Route("api/app/sales-orders")]
[Authorize(ArilyPermissions.SalesOrders.Default)]
public class SalesOrderController : ArilyController
{
    private readonly ISalesOrderAppService _appService;
    private readonly ILogger<SalesOrderController> _logger;

    public SalesOrderController(
        ISalesOrderAppService appService,
        ILogger<SalesOrderController> logger)
    {
        _appService = appService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [Authorize(ArilyPermissions.SalesOrders.Default)]
    public async Task<SalesOrderDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetSalesOrder: id={Id}", id);
        return await _appService.GetAsync(id);
    }

    [HttpGet]
    [Authorize(ArilyPermissions.SalesOrders.Default)]
    public async Task<PagedResultDto<SalesOrderDto>> GetListAsync([FromQuery] GetSalesOrderListInput input)
    {
        _logger.LogInformation("GetSalesOrderList: filter={Filter} customerId={CustomerId} status={Status}",
            input.Filter, input.CustomerId, input.Status);
        return await _appService.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.SalesOrders.Create)]
    public async Task<SalesOrderDto> CreateAsync([FromBody] CreateUpdateSalesOrderDto input)
    {
        _logger.LogInformation("CreateSalesOrder: orderNo={OrderNo} customerId={CustomerId}", input.OrderNo, input.CustomerId);
        return await _appService.CreateAsync(input);
    }

    [HttpPut("{id:guid}")]
    [Authorize(ArilyPermissions.SalesOrders.Edit)]
    public async Task<SalesOrderDto> UpdateAsync(Guid id, [FromBody] CreateUpdateSalesOrderDto input)
    {
        _logger.LogInformation("UpdateSalesOrder: id={Id} orderNo={OrderNo}", id, input.OrderNo);
        return await _appService.UpdateAsync(id, input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.SalesOrders.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteSalesOrder: id={Id}", id);
        await _appService.DeleteAsync(id);
    }

    [HttpPost("{id:guid}/confirm")]
    [Authorize(ArilyPermissions.SalesOrders.Edit)]
    public async Task<SalesOrderDto> ConfirmAsync(Guid id)
    {
        _logger.LogInformation("ConfirmSalesOrder: id={Id}", id);
        return await _appService.ConfirmAsync(id);
    }

    [HttpPost("{id:guid}/deliver")]
    [Authorize(ArilyPermissions.SalesOrders.Edit)]
    public async Task<SalesOrderDto> DeliverAsync(Guid id)
    {
        _logger.LogInformation("DeliverSalesOrder: id={Id}", id);
        return await _appService.DeliverAsync(id);
    }

    [HttpPost("{id:guid}/complete")]
    [Authorize(ArilyPermissions.SalesOrders.Edit)]
    public async Task<SalesOrderDto> CompleteAsync(Guid id)
    {
        _logger.LogInformation("CompleteSalesOrder: id={Id}", id);
        return await _appService.CompleteAsync(id);
    }

    [HttpPost("{id:guid}/cancel")]
    [Authorize(ArilyPermissions.SalesOrders.Edit)]
    public async Task<SalesOrderDto> CancelAsync(Guid id)
    {
        _logger.LogInformation("CancelSalesOrder: id={Id}", id);
        return await _appService.CancelAsync(id);
    }
}
