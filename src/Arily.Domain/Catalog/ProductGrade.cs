using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Catalog;

/// <summary>Cấp chất lượng: Loại 1, Loại 2, Dạt...</summary>
public class ProductGrade : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid ProductId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int SortOrder { get; set; }
    public CommonStatus Status { get; set; }

    protected ProductGrade() { }

    public ProductGrade(Guid id, Guid? tenantId, Guid productId, string code, string name) : base(id)
    {
        TenantId = tenantId;
        ProductId = productId;
        Code = code;
        Name = name;
        Status = CommonStatus.Active;
    }
}
