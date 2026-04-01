using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arily.Redis;

/// <summary>
/// Abstraction cho Redis — String, Hash, List, Set, Sorted Set, Stream.
/// </summary>
public interface IRedisService
{
    // ── Key ───────────────────────────────────────────────────────────────────

    Task<bool> KeyExistsAsync(string key);
    Task KeyDeleteAsync(string key);
    Task KeyExpireAsync(string key, TimeSpan ttl);

    // ── String ────────────────────────────────────────────────────────────────

    Task<T?> StringGetAsync<T>(string key) where T : class;
    Task StringSetAsync<T>(string key, T value, TimeSpan ttl) where T : class;

    /// <summary>Atomic integer increment. Dùng cho counter, rate limit.</summary>
    Task<long> StringIncrementAsync(string key, long value = 1);

    /// <summary>
    /// Atomic float increment (INCRBYFLOAT).
    /// Dùng cho số lượng tồn kho lẻ (kg, tấn): cộng/trừ trực tiếp, không race condition.
    /// </summary>
    Task<double> StringIncrementFloatAsync(string key, double value);

    // ── Hash ──────────────────────────────────────────────────────────────────

    /// <summary>Lấy một field từ hash. Trả về null nếu key hoặc field không tồn tại.</summary>
    Task<T?> HashGetAsync<T>(string hashKey, string field) where T : class;

    /// <summary>
    /// Lấy toàn bộ values trong hash.
    /// Trả về null nếu hash key chưa tồn tại (cache miss).
    /// Trả về list rỗng nếu key tồn tại nhưng không có field nào.
    /// </summary>
    Task<List<T>?> HashGetAllAsync<T>(string hashKey) where T : class;

    /// <summary>Thêm / cập nhật một field trong hash, đồng thời refresh TTL của cả hash.</summary>
    Task HashSetAsync<T>(string hashKey, string field, T value, TimeSpan ttl) where T : class;

    /// <summary>Xoá một field khỏi hash.</summary>
    Task HashDeleteAsync(string hashKey, string field);

    Task<bool> HashExistsAsync(string hashKey, string field);

    /// <summary>Bulk-load toàn bộ collection vào hash (dùng khi cache miss).</summary>
    Task HashLoadAsync<T>(string hashKey, IReadOnlyList<(string Field, T Value)> items, TimeSpan ttl) where T : class;

    // ── List ──────────────────────────────────────────────────────────────────

    /// <summary>Push vào đầu list (LPUSH). Dùng cho notification queue, job queue.</summary>
    Task ListPushAsync<T>(string key, T value) where T : class;

    /// <summary>Pop từ cuối list (RPOP) — FIFO queue khi kết hợp với LPUSH.</summary>
    Task<T?> ListPopAsync<T>(string key) where T : class;

    /// <summary>Lấy range phần tử (LRANGE). 0, -1 = toàn bộ list.</summary>
    Task<List<T>> ListRangeAsync<T>(string key, long start = 0, long stop = -1) where T : class;

    Task<long> ListLengthAsync(string key);

    // ── Set ───────────────────────────────────────────────────────────────────

    Task SetAddAsync(string key, string value);
    Task SetRemoveAsync(string key, string value);
    Task<bool> SetContainsAsync(string key, string value);
    Task<List<string>> SetMembersAsync(string key);

    // ── Sorted Set ────────────────────────────────────────────────────────────
    // Score thường là số lượng tồn kho, giá, hoặc timestamp.
    // Ví dụ: ZADD arly:inv:low-stock 45.5 "lot-id-abc"
    //         → có thể query ZRANGEBYSCORE 0 50 để tìm tất cả lô < 50kg

    Task SortedSetAddAsync(string key, string member, double score);

    /// <summary>
    /// Atomic increment score của một member (ZINCRBY).
    /// Dùng để tăng/giảm tồn kho trong sorted set mà không cần GET rồi SET.
    /// </summary>
    Task<double> SortedSetIncrementAsync(string key, string member, double value);

    Task SortedSetRemoveAsync(string key, string member);

    /// <summary>Lấy các member có score trong khoảng [min, max] kèm theo score.</summary>
    Task<List<(string Member, double Score)>> SortedSetRangeByScoreAsync(
        string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity);

    /// <summary>Lấy score hiện tại của member. Null nếu member không tồn tại.</summary>
    Task<double?> SortedSetScoreAsync(string key, string member);

    // ── Stream ────────────────────────────────────────────────────────────────
    // Redis Stream ~ Kafka: persistent, consumer group, offset, có thể replay.
    // Khác Pub/Sub: message không mất khi không có consumer lắng nghe.
    //
    // Flow:
    //   Producer  → StreamAddAsync(key, fields)
    //   Consumer  → StreamReadGroupAsync(key, group, consumer, count)
    //              → xử lý message
    //              → StreamAckAsync(key, group, messageId)

    /// <summary>
    /// Tạo consumer group cho stream (cần gọi 1 lần trước khi đọc).
    /// mkStream = true: tạo stream nếu chưa tồn tại.
    /// </summary>
    Task StreamCreateGroupAsync(string key, string groupName, bool mkStream = true);

    /// <summary>
    /// Ghi một message vào stream (XADD).
    /// Trả về message ID do Redis sinh (format: {ms}-{seq}).
    /// </summary>
    Task<string> StreamAddAsync(string key, Dictionary<string, string> fields);

    /// <summary>
    /// Đọc các message chưa xử lý từ stream theo consumer group (XREADGROUP).
    /// Trả về list rỗng nếu không có message mới.
    /// </summary>
    Task<List<RedisStreamMessage>> StreamReadGroupAsync(
        string key, string groupName, string consumerName, int count = 10);

    /// <summary>Xác nhận đã xử lý xong message (XACK). Phải gọi sau khi xử lý thành công.</summary>
    Task StreamAckAsync(string key, string groupName, string messageId);
}
