using System;
using System.Threading.Tasks;
using Arily.Catalog.UnitOfMeasures;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Catalog;

[ApiController]
[Route("api/app/unit-of-measures")]
[Authorize(ArilyPermissions.UnitOfMeasures.Default)]
public class UnitOfMeasureController : ArilyController
{
    private readonly IUnitOfMeasureAppService _unitOfMeasureAppService;
    private readonly ILogger<UnitOfMeasureController> _logger;

    public UnitOfMeasureController(
        IUnitOfMeasureAppService unitOfMeasureAppService,
        ILogger<UnitOfMeasureController> logger)
    {
        _unitOfMeasureAppService = unitOfMeasureAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    [Authorize(ArilyPermissions.UnitOfMeasures.Default)]
    public async Task<UnitOfMeasureDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetUnitOfMeasure: id={Id}", id);
        return await _unitOfMeasureAppService.GetAsync(id);
    }

    [HttpGet]
    [Authorize(ArilyPermissions.UnitOfMeasures.Default)]
    public async Task<PagedResultDto<UnitOfMeasureDto>> GetListAsync([FromQuery] GetUnitOfMeasureListInput input)
    {
        _logger.LogInformation("GetUnitOfMeasureList: filter={Filter} status={Status} skip={Skip} max={Max}",
            input.Filter, input.Status, input.SkipCount, input.MaxResultCount);
        return await _unitOfMeasureAppService.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.UnitOfMeasures.Create)]
    public async Task<UnitOfMeasureDto> CreateAsync([FromBody] CreateUpdateUnitOfMeasureDto input)
    {
        _logger.LogInformation("CreateUnitOfMeasure: code={Code} name={Name}", input.Code, input.Name);
        return await _unitOfMeasureAppService.CreateAsync(input);
    }

    [HttpPut("{id:guid}")]
    [Authorize(ArilyPermissions.UnitOfMeasures.Edit)]
    public async Task<UnitOfMeasureDto> UpdateAsync(Guid id, [FromBody] CreateUpdateUnitOfMeasureDto input)
    {
        _logger.LogInformation("UpdateUnitOfMeasure: id={Id} code={Code} name={Name}", id, input.Code, input.Name);
        return await _unitOfMeasureAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.UnitOfMeasures.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteUnitOfMeasure: id={Id}", id);
        await _unitOfMeasureAppService.DeleteAsync(id);
    }
}
