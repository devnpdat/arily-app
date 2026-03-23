using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Catalog;

public class ProductCategory : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public CommonStatus Status { get; set; }

    protected ProductCategory() { }

    public ProductCategory(Guid id, Guid? tenantId, string code, string name) : base(id)
    {
        TenantId = tenantId;
        Code = code;
        Name = name;
        Status = CommonStatus.Active;
    }
}
