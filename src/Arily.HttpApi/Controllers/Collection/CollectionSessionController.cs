using System;
using System.Threading.Tasks;
using Arily.Collection.CollectionSessions;
using Arily.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.Collection;

[ApiController]
[Route("api/app/collection-sessions")]
[Authorize(ArilyPermissions.CollectionSessions.Default)]
public class CollectionSessionController : ArilyController
{
    private readonly ICollectionSessionAppService _sessionAppService;
    private readonly ILogger<CollectionSessionController> _logger;

    public CollectionSessionController(
        ICollectionSessionAppService sessionAppService,
        ILogger<CollectionSessionController> logger)
    {
        _sessionAppService = sessionAppService;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<CollectionSessionDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetCollectionSession: id={Id}", id);
        return await _sessionAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<CollectionSessionDto>> GetListAsync([FromQuery] GetCollectionSessionListInput input)
    {
        _logger.LogInformation("GetCollectionSessionList: filter={Filter} status={Status}", input.Filter, input.Status);
        return await _sessionAppService.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(ArilyPermissions.CollectionSessions.Create)]
    public async Task<CollectionSessionDto> CreateAsync([FromBody] CreateUpdateCollectionSessionDto input)
    {
        _logger.LogInformation("CreateCollectionSession: code={Code} name={Name}", input.Code, input.Name);
        return await _sessionAppService.CreateAsync(input);
    }

    [HttpPut("{id:guid}")]
    [Authorize(ArilyPermissions.CollectionSessions.Edit)]
    public async Task<CollectionSessionDto> UpdateAsync(Guid id, [FromBody] CreateUpdateCollectionSessionDto input)
    {
        _logger.LogInformation("UpdateCollectionSession: id={Id}", id);
        return await _sessionAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(ArilyPermissions.CollectionSessions.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteCollectionSession: id={Id}", id);
        await _sessionAppService.DeleteAsync(id);
    }

    [HttpPost("{id:guid}/open")]
    [Authorize(ArilyPermissions.CollectionSessions.Edit)]
    public async Task<CollectionSessionDto> OpenAsync(Guid id)
    {
        _logger.LogInformation("OpenCollectionSession: id={Id}", id);
        return await _sessionAppService.OpenAsync(id);
    }

    [HttpPost("{id:guid}/close")]
    [Authorize(ArilyPermissions.CollectionSessions.Edit)]
    public async Task<CollectionSessionDto> CloseAsync(Guid id)
    {
        _logger.LogInformation("CloseCollectionSession: id={Id}", id);
        return await _sessionAppService.CloseAsync(id);
    }
}
