using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arily.Auditing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Uow;

namespace Arily.Interceptor;

public class CustomAuditingInterceptor : AuditingInterceptor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CustomAuditingInterceptor(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();

        var auditingHelper = serviceScope.ServiceProvider.GetRequiredService<IAuditingHelper>();
        var auditingOptions = serviceScope.ServiceProvider.GetRequiredService<IOptions<AbpAuditingOptions>>().Value;
        var auditingResponseOptions = serviceScope.ServiceProvider.GetRequiredService<IOptions<CustomAuditLogOptions>>().Value;

        if (!ShouldIntercept(invocation, auditingOptions, auditingHelper))
        {
            await invocation.ProceedAsync();
            return;
        }

        var auditingManager = serviceScope.ServiceProvider.GetRequiredService<IAuditingManager>();

        if (auditingManager.Current != null)
        {
            await ProceedByLoggingAsync(invocation, auditingHelper, auditingManager.Current, auditingResponseOptions.IsEnabledLogResponse);
        }
        else
        {
            var currentUser = serviceScope.ServiceProvider.GetRequiredService<Volo.Abp.Users.ICurrentUser>();
            var unitOfWorkManager = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
            await ProcessWithNewAuditingScopeAsync(invocation, auditingOptions, currentUser, auditingManager, auditingHelper, unitOfWorkManager, auditingResponseOptions);
        }
    }

    private static async Task ProceedByLoggingAsync(
        IAbpMethodInvocation invocation,
        IAuditingHelper auditingHelper,
        IAuditLogScope auditLogScope,
        bool isEnabledLogResponse)
    {
        var auditLog = auditLogScope.Log;
        var declaringType = invocation.Method.DeclaringType;
        var implementsInterface = declaringType?.GetInterfaces().Contains(typeof(IEnableLogResponseAudit)) == true;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await invocation.ProceedAsync();
        }
        catch (Volo.Abp.BusinessException bex)
        {
            auditLog.Exceptions.Add(bex);
            throw;
        }
        catch (Exception ex)
        {
            auditLog.Exceptions.Add(ex);
            throw;
        }
        finally
        {
            AuditLogActionInfo auditLogAction;

            if (implementsInterface && isEnabledLogResponse)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Request: {SerializeParameters(invocation.Arguments)}");
                sb.AppendLine($"    Response: {SerializeParameters(new[] { invocation.ReturnValue })}");

                auditLogAction = new AuditLogActionInfo
                {
                    ServiceName = invocation.Method.DeclaringType?.FullName ?? string.Empty,
                    MethodName = invocation.Method.Name,
                    Parameters = sb.ToString(),
                    ExecutionTime = DateTime.Now,
                    ExecutionDuration = 0
                };
            }
            else
            {
                auditLogAction = auditingHelper.CreateAuditLogAction(
                    auditLog,
                    invocation.TargetObject.GetType(),
                    invocation.Method,
                    invocation.Arguments);
            }

            stopwatch.Stop();
            auditLogAction.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
            auditLog.Actions.Add(auditLogAction);
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

    private async Task ProcessWithNewAuditingScopeAsync(
        IAbpMethodInvocation invocation,
        AbpAuditingOptions options,
        Volo.Abp.Users.ICurrentUser currentUser,
        IAuditingManager auditingManager,
        IAuditingHelper auditingHelper,
        IUnitOfWorkManager unitOfWorkManager,
        CustomAuditLogOptions customAuditLogOptions)
    {
        var hasError = false;
        using var saveHandle = auditingManager.BeginScope();

        try
        {
            await ProceedByLoggingAsync(invocation, auditingHelper, auditingManager.Current!, customAuditLogOptions.IsEnabledLogResponse);

            Debug.Assert(auditingManager.Current != null);
            if (auditingManager.Current.Log.Exceptions.Any())
            {
                hasError = true;
            }
        }
        catch (Exception)
        {
            hasError = true;
            throw;
        }
        finally
        {
            if (ShouldWriteAuditLog(invocation, options, currentUser, hasError))
            {
                if (unitOfWorkManager.Current != null)
                {
                    try
                    {
                        await unitOfWorkManager.Current.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        if (auditingManager.Current != null && !auditingManager.Current.Log.Exceptions.Contains(ex))
                        {
                            auditingManager.Current.Log.Exceptions.Add(ex);
                        }
                    }
                }

                await saveHandle.SaveAsync();
            }
        }
    }

    private static bool ShouldWriteAuditLog(
        IAbpMethodInvocation invocation,
        AbpAuditingOptions options,
        Volo.Abp.Users.ICurrentUser currentUser,
        bool hasError)
    {
        if (options.AlwaysLogOnException && hasError)
        {
            return true;
        }

        if (!options.IsEnabledForAnonymousUsers && !currentUser.IsAuthenticated)
        {
            return false;
        }

        if (!options.IsEnabledForGetRequests &&
            invocation.Method.Name.StartsWith("Get", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
}
