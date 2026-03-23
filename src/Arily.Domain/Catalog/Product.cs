using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Catalog;

public class Product : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid ProductCategoryId { get; set; }
    public Guid UnitOfMeasureId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    /// <summary>Tỷ lệ hao hụt mặc định (0.00-1.00)</summary>
    public decimal DefaultLossRate { get; set; }

    public string? Note { get; set; }
    public CommonStatus Status { get; set; }

    protected Product() { }

    public Product(Guid id, Guid? tenantId, Guid productCategoryId, Guid unitOfMeasureId, string code, string name) : base(id)
    {
        TenantId = tenantId;
        ProductCategoryId = productCategoryId;
        UnitOfMeasureId = unitOfMeasureId;
        Code = code;
        Name = name;
        DefaultLossRate = 0;
        Status = CommonStatus.Active;
    }
}
