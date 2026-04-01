using System;

namespace Arily.Redis;

/// <summary>
/// Redis key builder cho master data.
///
/// String key  →  arly:md:{entity}:{tenantId}:{id}         (single record)
/// Hash key    →  arly:md:{entity}:{tenantId}:list          (collection, field = entityId)
/// </summary>
public static class RedisKeys
{
    private const string P = "arly:md:";

    // ── Catalog ───────────────────────────────────────────────────────────────
    public static string ProductCategory(Guid? tenantId, Guid id)         => $"{P}product-category:{tenantId ?? Guid.Empty}:{id}";
    public static string ProductCategoryList(Guid? tenantId)               => $"{P}product-category:{tenantId ?? Guid.Empty}:list";

    public static string UnitOfMeasure(Guid? tenantId, Guid id)           => $"{P}uom:{tenantId ?? Guid.Empty}:{id}";
    public static string UnitOfMeasureList(Guid? tenantId)                 => $"{P}uom:{tenantId ?? Guid.Empty}:list";

    public static string Product(Guid? tenantId, Guid id)                 => $"{P}product:{tenantId ?? Guid.Empty}:{id}";
    public static string ProductList(Guid? tenantId)                       => $"{P}product:{tenantId ?? Guid.Empty}:list";

    /// <param name="productId">Cache list bị scope theo product</param>
    public static string ProductGrade(Guid? tenantId, Guid id)            => $"{P}product-grade:{tenantId ?? Guid.Empty}:{id}";
    public static string ProductGradeList(Guid? tenantId, Guid productId) => $"{P}product-grade:{tenantId ?? Guid.Empty}:{productId}:list";

    // ── CRM ───────────────────────────────────────────────────────────────────
    public static string Farmer(Guid? tenantId, Guid id)                  => $"{P}farmer:{tenantId ?? Guid.Empty}:{id}";
    public static string FarmerList(Guid? tenantId)                        => $"{P}farmer:{tenantId ?? Guid.Empty}:list";

    /// <param name="farmerId">Cache list bị scope theo farmer</param>
    public static string FarmerGarden(Guid? tenantId, Guid id)            => $"{P}farmer-garden:{tenantId ?? Guid.Empty}:{id}";
    public static string FarmerGardenList(Guid? tenantId, Guid farmerId)  => $"{P}farmer-garden:{tenantId ?? Guid.Empty}:{farmerId}:list";

    public static string Customer(Guid? tenantId, Guid id)                => $"{P}customer:{tenantId ?? Guid.Empty}:{id}";
    public static string CustomerList(Guid? tenantId)                      => $"{P}customer:{tenantId ?? Guid.Empty}:list";

    // ── Inventory ─────────────────────────────────────────────────────────────
    public static string Warehouse(Guid? tenantId, Guid id)               => $"{P}warehouse:{tenantId ?? Guid.Empty}:{id}";
    public static string WarehouseList(Guid? tenantId)                     => $"{P}warehouse:{tenantId ?? Guid.Empty}:list";
}
