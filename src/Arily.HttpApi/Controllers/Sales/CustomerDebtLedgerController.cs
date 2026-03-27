using System;
using System.Threading.Tasks;
using Arily.Permissions;
using Arily.Sales.CustomerDebtLedgers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Sales;

[ApiController]
[Route("api/app/customer-debt-ledgers")]
[Authorize(ArilyPermissions.CustomerDebtLedgers.Default)]
public class CustomerDebtLedgerController : ArilyController
{
    private readonly ICustomerDebtLedgerAppService _appService;
    private readonly ILogger<CustomerDebtLedgerController> _logger;

    public CustomerDebtLedgerController(
        ICustomerDebtLedgerAppService appService,
        ILogger<CustomerDebtLedgerController> logger)
    {
        _appService = appService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [Authorize(ArilyPermissions.CustomerDebtLedgers.Default)]
    public async Task<CustomerDebtLedgerDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetCustomerDebtLedger: id={Id}", id);
        return await _appService.GetAsync(id);
    }

    [HttpGet]
    [Authorize(ArilyPermissions.CustomerDebtLedgers.Default)]
    public async Task<PagedResultDto<CustomerDebtLedgerDto>> GetListAsync([FromQuery] GetCustomerDebtLedgerListInput input)
    {
        _logger.LogInformation("GetCustomerDebtLedgerList: customerId={CustomerId} ledgerType={LedgerType} dateFrom={DateFrom} dateTo={DateTo}",
            input.CustomerId, input.LedgerType, input.DateFrom, input.DateTo);
        return await _appService.GetListAsync(input);
    }
}
