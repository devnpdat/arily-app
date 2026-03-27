using System;
using System.Threading.Tasks;
using Arily.Finance.FarmerDebtLedgers;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Finance;

[ApiController]
[Route("api/app/farmer-debt-ledgers")]
[Authorize(ArilyPermissions.FarmerDebtLedgers.Default)]
public class FarmerDebtLedgerController : ArilyController
{
    private readonly IFarmerDebtLedgerAppService _appService;
    private readonly ILogger<FarmerDebtLedgerController> _logger;

    public FarmerDebtLedgerController(
        IFarmerDebtLedgerAppService appService,
        ILogger<FarmerDebtLedgerController> logger)
    {
        _appService = appService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [Authorize(ArilyPermissions.FarmerDebtLedgers.Default)]
    public async Task<FarmerDebtLedgerDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetFarmerDebtLedger: id={Id}", id);
        return await _appService.GetAsync(id);
    }

    [HttpGet]
    [Authorize(ArilyPermissions.FarmerDebtLedgers.Default)]
    public async Task<PagedResultDto<FarmerDebtLedgerDto>> GetListAsync([FromQuery] GetFarmerDebtLedgerListInput input)
    {
        _logger.LogInformation("GetFarmerDebtLedgerList: farmerId={FarmerId} ledgerType={LedgerType} dateFrom={DateFrom} dateTo={DateTo}",
            input.FarmerId, input.LedgerType, input.DateFrom, input.DateTo);
        return await _appService.GetListAsync(input);
    }
}
