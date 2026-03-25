using System;
using System.Threading.Tasks;
using Arily.AuditLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Dtos;

namespace Arily.Controllers.AuditLog;

[ApiController]
[Route("api/app/audit-logs")]
[Authorize(Roles = "admin")]
public class AuditLogController : ArilyController
{
    private readonly IAuditLogAppService _auditLogAppService;
    private readonly ILogger<AuditLogController> _logger;

    public AuditLogController(IAuditLogAppService auditLogAppService, ILogger<AuditLogController> logger)
    {
        _auditLogAppService = auditLogAppService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<PagedResultDto<AuditLogDto>> GetListAsync([FromQuery] GetAuditLogListInput input)
    {
        _logger.LogInformation("GetAuditLogs: startTime={Start} endTime={End} user={User} method={Method} status={Status}",
            input.StartTime, input.EndTime, input.UserName, input.HttpMethod, input.HttpStatusCode);
        return await _auditLogAppService.GetListAsync(input);
    }

    [HttpGet("{id:guid}")]
    public async Task<AuditLogDto> GetAsync(Guid id)
    {
        _logger.LogInformation("GetAuditLog: id={Id}", id);
        return await _auditLogAppService.GetAsync(id);
    }
}
