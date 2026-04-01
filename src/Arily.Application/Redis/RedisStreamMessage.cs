using System.Collections.Generic;

namespace Arily.Redis;

/// <summary>Một message đọc từ Redis Stream.</summary>
public class RedisStreamMessage
{
    /// <summary>ID tự sinh bởi Redis, format: {unix-ms}-{sequence}, ví dụ: 1711234567890-0</summary>
    public string Id { get; set; } = null!;

    public Dictionary<string, string> Fields { get; set; } = new();
}
