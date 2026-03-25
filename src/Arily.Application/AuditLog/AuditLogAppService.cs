using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AuditLogging;

namespace Arily.AuditLog;

[Authorize(Roles = "admin")]
public class AuditLogAppService : ArilyAppService, IAuditLogAppService
{
    private readonly IAuditLogRepository _auditLogRepository;

    public AuditLogAppService(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    public async Task<PagedResultDto<AuditLogDto>> GetListAsync(GetAuditLogListInput input)
    {
        var totalCount = await _auditLogRepository.GetCountAsync(
            startTime: input.StartTime,
            endTime: input.EndTime,
            userName: input.UserName,
            httpMethod: input.HttpMethod,
            url: input.Url,
            httpStatusCode: input.HttpStatusCode.HasValue ? (HttpStatusCode?)input.HttpStatusCode.Value : null,
            hasException: input.HasException
        );

        var logs = await _auditLogRepository.GetListAsync(
            sorting: input.Sorting ?? "executionTime desc",
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount,
            startTime: input.StartTime,
            endTime: input.EndTime,
            userName: input.UserName,
            httpMethod: input.HttpMethod,
            url: input.Url,
            httpStatusCode: input.HttpStatusCode.HasValue ? (HttpStatusCode?)input.HttpStatusCode.Value : null,
            hasException: input.HasException,
            includeDetails: true
        );

        return new PagedResultDto<AuditLogDto>(totalCount, MapToDto(logs));
    }

    public async Task<AuditLogDto> GetAsync(Guid id)
    {
        var log = await _auditLogRepository.GetAsync(id, includeDetails: true);
        return MapToDto(log);
    }

    private List<AuditLogDto> MapToDto(List<Volo.Abp.AuditLogging.AuditLog> logs)
        => logs.Select(MapToDto).ToList();

    private AuditLogDto MapToDto(Volo.Abp.AuditLogging.AuditLog log) => new()
    {
        Id = log.Id,
        ApplicationName = log.ApplicationName,
        UserId = log.UserId,
        UserName = log.UserName,
        ClientIpAddress = log.ClientIpAddress,
        HttpMethod = log.HttpMethod,
        Url = log.Url,
        HttpStatusCode = log.HttpStatusCode ?? 0,
        ExecutionDuration = log.ExecutionDuration,
        ExecutionTime = log.ExecutionTime,
        Exceptions = log.Exceptions,
        Actions = log.Actions.Select(a => new AuditLogActionDto
        {
            ServiceName = a.ServiceName,
            MethodName = a.MethodName,
            Parameters = a.Parameters,
            ExecutionDuration = a.ExecutionDuration,
            ExecutionTime = a.ExecutionTime
        }).ToList(),
        EntityChanges = log.EntityChanges.Select(e => new EntityChangeDto
        {
            EntityTypeFullName = e.EntityTypeFullName,
            EntityId = e.EntityId,
            ChangeType = (byte)e.ChangeType,
            ChangeTime = e.ChangeTime
        }).ToList()
    };
}
