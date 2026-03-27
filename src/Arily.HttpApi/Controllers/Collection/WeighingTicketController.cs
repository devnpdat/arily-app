using System;
using System.Threading.Tasks;
using Arily.Collection.WeighingTickets;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Collection;

[ApiController]
[Route("api/app/weighing-tickets")]
[Authorize(ArilyPermissions.WeighingTickets.Default)]
public class WeighingTicketController : ArilyController
{
    private readonly IWeighingTicketAppService _weighingTicketAppService;
    private readonly ILogger<WeighingTicketController> _logger;

    public WeighingTicketController(
        IWeighingTicketAppService weighingTicketAppService,
        ILogger<WeighingTicketController> logger)
    {
        _weighingTicketAppService = weighingTicketAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<WeighingTicketDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetWeighingTicket: id={Id}", id);
        return await _weighingTicketAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<ListResultDto<WeighingTicketDto>> GetListAsync([FromQuery] Guid purchaseOrderId)
    {
        _logger.LogInformation("GetWeighingTicketList: purchaseOrderId={PurchaseOrderId}", purchaseOrderId);
        return await _weighingTicketAppService.GetListAsync(purchaseOrderId);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.WeighingTickets.Create)]
    public async Task<WeighingTicketDto> CreateAsync([FromBody] CreateWeighingTicketDto input)
    {
        _logger.LogInformation("CreateWeighingTicket: purchaseOrderId={PurchaseOrderId} ticketNo={TicketNo}",
            input.PurchaseOrderId, input.TicketNo);
        return await _weighingTicketAppService.CreateAsync(input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.WeighingTickets.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteWeighingTicket: id={Id}", id);
        await _weighingTicketAppService.DeleteAsync(id);
    }
}
