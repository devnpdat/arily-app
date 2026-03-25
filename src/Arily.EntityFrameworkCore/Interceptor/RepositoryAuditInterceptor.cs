using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Arily.Auditing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Arily.EntityFrameworkCore.Interceptor;

/// <summary>
/// Ghi log các method của Repository vào AuditLog.
/// Đăng ký qua context.Services.OnRegistred trong ArilyEntityFrameworkCoreModule.
/// </summary>
public class RepositoryAuditInterceptor : IAbpInterceptor, ITransientDependency
{
    private readonly IAuditingManager _auditingManager;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RepositoryAuditInterceptor(IAuditingManager auditingManager, IServiceScopeFactory serviceScopeFactory)
    {
        _auditingManager = auditingManager;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();

        var auditingOptions = serviceScope.ServiceProvider.GetRequiredService<IOptions<AbpAuditingOptions>>().Value;
        var auditingResponseOptions = serviceScope.ServiceProvider.GetRequiredService<IOptions<CustomAuditLogOptions>>().Value;

        if (!auditingOptions.IsEnabled)
        {
            await invocation.ProceedAsync();
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        var executionTime = DateTime.Now;

        try
        {
            await invocation.ProceedAsync();
        }
        finally
        {
            stopwatch.Stop();

            if (_auditingManager?.Current?.Log?.Actions != null)
            {
                var declaringTypeFullName = invocation.Method.DeclaringType?.FullName ?? string.Empty;

                if (declaringTypeFullName.Contains("Repository"))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Request: {SerializeParameters(invocation.Arguments)}");

                    if (auditingResponseOptions.IsEnabledLogResponse)
                    {
                        sb.AppendLine($"    Response: {SerializeParameters(new[] { invocation.ReturnValue })}");
                    }

                    _auditingManager.Current.Log.Actions.Add(new AuditLogActionInfo
                    {
                        ServiceName = declaringTypeFullName,
                        MethodName = invocation.Method.Name,
                        Parameters = sb.ToString(),
                        ExecutionTime = executionTime,
                        ExecutionDuration = (int)stopwatch.ElapsedMilliseconds
                    });
                }
            }
        }
    }

    private static string SerializeParameters(object[] arguments)
    {
        try
        {
            return JsonConvert.SerializeObject(arguments);
        }
        catch
        {
            return string.Empty;
        }
    }
}
