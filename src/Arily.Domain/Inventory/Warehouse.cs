using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Inventory;

public class Warehouse : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? ProvinceCode { get; set; }
    public string? Note { get; set; }
    public CommonStatus Status { get; set; }

    protected Warehouse() { }

    public Warehouse(Guid id, Guid? tenantId, string code, string name) : base(id)
    {
        TenantId = tenantId;
        Code = code;
        Name = name;
        Status = CommonStatus.Active;
    }
}
