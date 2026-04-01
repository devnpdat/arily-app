using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;
using Volo.Abp.DependencyInjection;

namespace Arily.Redis;

public class RedisService : IRedisService, ISingletonDependency
{
    private readonly IDatabase _db;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public RedisService(IConnectionMultiplexer multiplexer)
    {
        _db = multiplexer.GetDatabase();
    }

    // ── Key ───────────────────────────────────────────────────────────────────

    public async Task<bool> KeyExistsAsync(string key)
        => await _db.KeyExistsAsync(key);

    public async Task KeyDeleteAsync(string key)
        => await _db.KeyDeleteAsync(key);

    public async Task KeyExpireAsync(string key, TimeSpan ttl)
        => await _db.KeyExpireAsync(key, ttl);

    // ── String ────────────────────────────────────────────────────────────────

    public async Task<T?> StringGetAsync<T>(string key) where T : class
    {
        var value = await _db.StringGetAsync(key);
        return value.IsNullOrEmpty ? null : JsonSerializer.Deserialize<T>(value!, JsonOptions);
    }

    public async Task StringSetAsync<T>(string key, T value, TimeSpan ttl) where T : class
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        await _db.StringSetAsync(key, json, ttl);
    }

    public async Task<long> StringIncrementAsync(string key, long value = 1)
        => await _db.StringIncrementAsync(key, value);

    public async Task<double> StringIncrementFloatAsync(string key, double value)
        => await _db.StringIncrementAsync(key, value);

    // ── Hash ──────────────────────────────────────────────────────────────────

    public async Task<T?> HashGetAsync<T>(string hashKey, string field) where T : class
    {
        var value = await _db.HashGetAsync(hashKey, field);
        return value.IsNullOrEmpty ? null : JsonSerializer.Deserialize<T>(value!, JsonOptions);
    }

    public async Task<List<T>?> HashGetAllAsync<T>(string hashKey) where T : class
    {
        // Pipeline: EXISTS + HGETALL trong 1 round-trip
        var batch = _db.CreateBatch();
        var existsTask = batch.KeyExistsAsync(hashKey);
        var entriesTask = batch.HashGetAllAsync(hashKey);
        batch.Execute();

        if (!await existsTask)
        {
            return null;
        }

        var entries = await entriesTask;
        var result = new List<T>(entries.Length);
        foreach (var entry in entries)
        {
            if (entry.Name == "__sentinel" || entry.Value.IsNullOrEmpty)
            {
                continue;
            }

            var item = JsonSerializer.Deserialize<T>(entry.Value!, JsonOptions);
            if (item != null)
            {
                result.Add(item);
            }
        }
        return result;
    }

    public async Task HashSetAsync<T>(string hashKey, string field, T value, TimeSpan ttl) where T : class
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        var batch = _db.CreateBatch();
        _ = batch.HashSetAsync(hashKey, field, json);
        _ = batch.KeyExpireAsync(hashKey, ttl);
        batch.Execute();
        await Task.CompletedTask;
    }

    public async Task HashDeleteAsync(string hashKey, string field)
        => await _db.HashDeleteAsync(hashKey, field);

    public async Task<bool> HashExistsAsync(string hashKey, string field)
        => await _db.HashExistsAsync(hashKey, field);

    public async Task HashLoadAsync<T>(string hashKey, IReadOnlyList<(string Field, T Value)> items, TimeSpan ttl) where T : class
    {
        HashEntry[] entries;

        if (items.Count > 0)
        {
            entries = items
                .Select(x => new HashEntry(x.Field, JsonSerializer.Serialize(x.Value, JsonOptions)))
                .ToArray();
        }
        else
        {
            // Sentinel để phân biệt "loaded, 0 items" với "chưa load"
            entries = [new HashEntry("__sentinel", "1")];
        }

        var batch = _db.CreateBatch();
        _ = batch.HashSetAsync(hashKey, entries);
        _ = batch.KeyExpireAsync(hashKey, ttl);
        batch.Execute();
        await Task.CompletedTask;
    }

    // ── List ──────────────────────────────────────────────────────────────────

    public async Task ListPushAsync<T>(string key, T value) where T : class
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        await _db.ListLeftPushAsync(key, json);
    }

    public async Task<T?> ListPopAsync<T>(string key) where T : class
    {
        var value = await _db.ListRightPopAsync(key);
        return value.IsNullOrEmpty ? null : JsonSerializer.Deserialize<T>(value!, JsonOptions);
    }

    public async Task<List<T>> ListRangeAsync<T>(string key, long start = 0, long stop = -1) where T : class
    {
        var values = await _db.ListRangeAsync(key, start, stop);
        return values
            .Where(v => !v.IsNullOrEmpty)
            .Select(v => JsonSerializer.Deserialize<T>(v!, JsonOptions))
            .Where(x => x != null)
            .Cast<T>()
            .ToList();
    }

    public async Task<long> ListLengthAsync(string key)
        => await _db.ListLengthAsync(key);

    // ── Set ───────────────────────────────────────────────────────────────────

    public async Task SetAddAsync(string key, string value)
        => await _db.SetAddAsync(key, value);

    public async Task SetRemoveAsync(string key, string value)
        => await _db.SetRemoveAsync(key, value);

    public async Task<bool> SetContainsAsync(string key, string value)
        => await _db.SetContainsAsync(key, value);

    public async Task<List<string>> SetMembersAsync(string key)
    {
        var members = await _db.SetMembersAsync(key);
        return members.Select(m => m.ToString()).ToList();
    }

    // ── Sorted Set ────────────────────────────────────────────────────────────

    public async Task SortedSetAddAsync(string key, string member, double score)
        => await _db.SortedSetAddAsync(key, member, score);

    public async Task<double> SortedSetIncrementAsync(string key, string member, double value)
        => await _db.SortedSetIncrementAsync(key, member, value);

    public async Task SortedSetRemoveAsync(string key, string member)
        => await _db.SortedSetRemoveAsync(key, member);

    public async Task<List<(string Member, double Score)>> SortedSetRangeByScoreAsync(
        string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity)
    {
        var entries = await _db.SortedSetRangeByScoreWithScoresAsync(key, min, max);
        return entries.Select(e => (e.Element.ToString(), e.Score)).ToList();
    }

    public async Task<double?> SortedSetScoreAsync(string key, string member)
        => await _db.SortedSetScoreAsync(key, member);

    // ── Stream ────────────────────────────────────────────────────────────────

    public async Task StreamCreateGroupAsync(string key, string groupName, bool mkStream = true)
    {
        try
        {
            await _db.StreamCreateConsumerGroupAsync(key, groupName, "$", mkStream);
        }
        catch (RedisException ex) when (ex.Message.Contains("BUSYGROUP"))
        {
            // Group đã tồn tại — bỏ qua
        }
    }

    public async Task<string> StreamAddAsync(string key, Dictionary<string, string> fields)
    {
        var entries = fields.Select(kv => new NameValueEntry(kv.Key, kv.Value)).ToArray();
        var id = await _db.StreamAddAsync(key, entries);
        return id.ToString();
    }

    public async Task<List<RedisStreamMessage>> StreamReadGroupAsync(
        string key, string groupName, string consumerName, int count = 10)
    {
        var entries = await _db.StreamReadGroupAsync(key, groupName, consumerName, count: count);
        return entries.Select(e => new RedisStreamMessage
        {
            Id = e.Id.ToString(),
            Fields = e.Values.ToDictionary(v => v.Name.ToString(), v => v.Value.ToString())
        }).ToList();
    }

    public async Task StreamAckAsync(string key, string groupName, string messageId)
        => await _db.StreamAcknowledgeAsync(key, groupName, messageId);
}
