using System;

namespace Arily.Redis;

public static class RedisTtl
{
    /// <summary>Master data (danh mục, nông dân, khách hàng, kho) — ít thay đổi</summary>
    public static readonly TimeSpan MasterData = TimeSpan.FromHours(24);
}
