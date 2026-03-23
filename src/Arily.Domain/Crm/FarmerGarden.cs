using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Crm;

public class FarmerGarden : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid FarmerId { get; set; }
    public Guid ProductCategoryId { get; set; }
    public string GardenName { get; set; } = null!;

    /// <summary>Diện tích (hectare)</summary>
    public decimal? AreaHectare { get; set; }

    /// <summary>Sản lượng ước tính (kg)</summary>
    public decimal? EstimatedYieldKg { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
    public CommonStatus Status { get; set; }

    protected FarmerGarden() { }

    public FarmerGarden(Guid id, Guid? tenantId, Guid farmerId, Guid productCategoryId, string gardenName) : base(id)
    {
        TenantId = tenantId;
        FarmerId = farmerId;
        ProductCategoryId = productCategoryId;
        GardenName = gardenName;
        Status = CommonStatus.Active;
    }
}
