using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace Arily.Auditing;

/// <summary>
/// Ghi audit log ra Serilog thay vì lưu vào database.
/// Format: AUDIT LOG: {auditLog}||{executionDuration}||{traceId}||{correlationId}
/// </summary>
public class ArilyAuditingStore : IAuditingStore, ISingletonDependency
{
    private readonly ILogger<ArilyAuditingStore> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ArilyAuditingStore(ILogger<ArilyAuditingStore> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task SaveAsync(AuditLogInfo auditInfo)
    {
        var traceId = GetTraceId();

        if (auditInfo.Exceptions != null && auditInfo.Exceptions.Count > 0)
        {
            _logger.LogError(
                "AUDIT LOG: {AuditLog}||{ExecutionDuration}||{TraceId}||{CorrelationId}",
                auditInfo.ToString(), auditInfo.ExecutionDuration, traceId, auditInfo.CorrelationId);
        }
        else
        {
            _logger.LogInformation(
                "AUDIT LOG: {AuditLog}||{ExecutionDuration}||{TraceId}||{CorrelationId}",
                auditInfo.ToString(), auditInfo.ExecutionDuration, traceId, auditInfo.CorrelationId);
        }

        return Task.CompletedTask;
    }

    private string GetTraceId()
    {
        try
        {
            var context = _httpContextAccessor.HttpContext;

            if (context != null && context.Request.Headers.TryGetValue("X-Trace-Id", out var xTraceId))
            {
                return xTraceId.ToString();
            }

            if (System.Diagnostics.Activity.Current != null)
            {
                return System.Diagnostics.Activity.Current.TraceId.ToString();
            }

            return Guid.NewGuid().ToString("N");
        }
        catch
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
