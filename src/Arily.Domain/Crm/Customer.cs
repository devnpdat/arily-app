using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Crm;

public class Customer : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Code { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }

    /// <summary>Loại khách: Vựa, Chợ đầu mối, Siêu thị, Doanh nghiệp</summary>
    public string? CustomerType { get; set; }

    public string? ProvinceCode { get; set; }
    public string? DistrictCode { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
    public CommonStatus Status { get; set; }

    protected Customer() { }

    public Customer(Guid id, Guid? tenantId, string code, string fullName) : base(id)
    {
        TenantId = tenantId;
        Code = code;
        FullName = fullName;
        Status = CommonStatus.Active;
    }
}
