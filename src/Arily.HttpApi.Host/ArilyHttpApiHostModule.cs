using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Arily.EntityFrameworkCore;
using Arily.MultiTenancy;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Serilog;
using Arily.Auditing;
using Arily.Elasticsearch;
using Arily.Interceptor;
using Elastic.Apm.NetCoreAll;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;

namespace Arily;

[DependsOn(
    typeof(ArilyHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(ArilyApplicationModule),
    typeof(ArilyEntityFrameworkCoreModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class ArilyHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("Arily");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        ConfigureAuthentication(context);
        ConfigureAuditLogging(context, configuration);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);

        context.Services.AddHttpClient();
        context.Services.AddHostedService<ElasticsearchSetupService>();
        context.Services.AddAllElasticApm();
    }

    private void ConfigureAuditLogging(ServiceConfigurationContext context, IConfiguration configuration)
    {
        Configure<AbpAuditingOptions>(options =>
        {
            options.IsEnabled = bool.Parse(configuration["AbpAuditingOptions:IsEnabled"]!);
            options.HideErrors = bool.Parse(configuration["AbpAuditingOptions:HideErrors"]!);
            options.IsEnabledForAnonymousUsers = bool.Parse(configuration["AbpAuditingOptions:IsEnabledForAnonymousUsers"]!);
            options.AlwaysLogOnException = bool.Parse(configuration["AbpAuditingOptions:AlwaysLogOnException"]!);
            options.IsEnabledForGetRequests = bool.Parse(configuration["AbpAuditingOptions:IsEnabledForGetRequests"]!);
            options.ApplicationName = configuration["AbpAuditingOptions:ApplicationName"];
            options.EntityHistorySelectors.AddAllEntities();
        });

        Configure<CustomAuditLogOptions>(options =>
        {
            options.IsEnabledLogResponse = bool.Parse(configuration["AbpAuditingOptions:IsEnabledLogResponse"]!);
        });

        context.Services.Replace(
            ServiceDescriptor.Transient<AuditingInterceptor, CustomAuditingInterceptor>()
        );
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
                { "Arily", "Arily API" }
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Arily API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding", "X-Abp-Remote-Service-Error-Code", "X-Abp-Remote-Service-Error-Message", "X-Abp-Error-Code", "X-Abp-Error-Details")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();
        app.UseCorrelationId();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arily API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            c.OAuthScopes("Arily");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
