using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Arily.Elasticsearch;

/// <summary>
/// Chạy một lần khi app khởi động:
/// 1. Tạo ILM policy xoá log sau N ngày (mặc định 7)
/// 2. Tạo index template áp dụng policy cho tất cả index "arily-logs-*"
/// </summary>
public class ElasticsearchSetupService : IHostedService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ElasticsearchSetupService> _logger;
    private readonly string _esUrl;
    private readonly int _retentionDays;
    private const string PolicyName = "arily-logs-policy";
    private const string TemplateName = "arily-logs-template";
    private const string IndexPattern = "arily-logs-*";

    public ElasticsearchSetupService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<ElasticsearchSetupService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _esUrl = configuration["Elasticsearch:Url"] ?? "http://localhost:9200";
        _retentionDays = int.Parse(configuration["Elasticsearch:LogRetentionDays"] ?? "7");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Setting up Elasticsearch ILM policy (retention={Days} days)...", _retentionDays);
            await CreateIlmPolicyAsync(cancellationToken);
            await CreateIndexTemplateAsync(cancellationToken);
            _logger.LogInformation("Elasticsearch setup completed.");
        }
        catch (Exception ex)
        {
            // Không throw — ES có thể chưa sẵn sàng, app vẫn chạy được
            _logger.LogWarning(ex, "Elasticsearch setup failed. Logs may not be written to ES.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>
    /// PUT /_ilm/policy/arily-logs-policy
    /// Policy: hot phase → delete sau N ngày
    /// </summary>
    private async Task CreateIlmPolicyAsync(CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();
        var url = $"{_esUrl}/_ilm/policy/{PolicyName}";

        var policy = $$"""
        {
          "policy": {
            "phases": {
              "hot": {
                "min_age": "0ms",
                "actions": {
                  "rollover": {
                    "max_age": "1d",
                    "max_size": "50gb"
                  }
                }
              },
              "delete": {
                "min_age": "{{_retentionDays}}d",
                "actions": {
                  "delete": {}
                }
              }
            }
          }
        }
        """;

        var response = await client.PutAsync(
            url,
            new StringContent(policy, Encoding.UTF8, "application/json"),
            cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("ILM policy '{Policy}' created/updated (delete after {Days}d).", PolicyName, _retentionDays);
        }
        else
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning("ILM policy setup failed: {Status} — {Body}", response.StatusCode, body);
        }
    }

    /// <summary>
    /// PUT /_index_template/arily-logs-template
    /// Áp dụng ILM policy cho tất cả index khớp "arily-logs-*"
    /// </summary>
    private async Task CreateIndexTemplateAsync(CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();
        var url = $"{_esUrl}/_index_template/{TemplateName}";

        var template = $$"""
        {
          "index_patterns": ["{{IndexPattern}}"],
          "template": {
            "settings": {
              "number_of_shards": 1,
              "number_of_replicas": 0,
              "lifecycle.name": "{{PolicyName}}"
            },
            "mappings": {
              "properties": {
                "@timestamp":      { "type": "date" },
                "level":           { "type": "keyword" },
                "message":         { "type": "text" },
                "messageTemplate": { "type": "text" },
                "Application":     { "type": "keyword" },
                "Environment":     { "type": "keyword" },
                "TraceId":         { "type": "keyword" },
                "CorrelationId":   { "type": "keyword" },
                "ExecutionDuration": { "type": "integer" },
                "exception": {
                  "type": "object",
                  "properties": {
                    "message": { "type": "text" },
                    "type":    { "type": "keyword" }
                  }
                }
              }
            }
          },
          "priority": 100
        }
        """;

        var response = await client.PutAsync(
            url,
            new StringContent(template, Encoding.UTF8, "application/json"),
            cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Index template '{Template}' created/updated for pattern '{Pattern}'.", TemplateName, IndexPattern);
        }
        else
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning("Index template setup failed: {Status} — {Body}", response.StatusCode, body);
        }
    }
}
