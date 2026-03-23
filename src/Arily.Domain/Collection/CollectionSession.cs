using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Collection;

/// <summary>Phiên gom hàng. Ví dụ: "Sầu riêng Tiền Giang 23/03"</summary>
public class CollectionSession : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime SessionDate { get; set; }
    public Guid ProductCategoryId { get; set; }
    public string? RegionProvinceCode { get; set; }
    public string? RegionDistrictCode { get; set; }

    /// <summary>Người phụ trách gom hàng</summary>
    public Guid? BuyerUserId { get; set; }

    public CollectionSessionStatus Status { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? Note { get; set; }

    protected CollectionSession() { }

    public CollectionSession(Guid id, Guid? tenantId, string code, string name, DateTime sessionDate, Guid productCategoryId) : base(id)
    {
        TenantId = tenantId;
        Code = code;
        Name = name;
        SessionDate = sessionDate;
        ProductCategoryId = productCategoryId;
        Status = CollectionSessionStatus.Draft;
    }
}
