using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Crm;

public class Farmer : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Code { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? NickName { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string? ProvinceCode { get; set; }
    public string? DistrictCode { get; set; }
    public string? WardCode { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    /// <summary>Điểm uy tín nội bộ (0–100)</summary>
    public int ReputationScore { get; set; }

    public string? Note { get; set; }
    public FarmerStatus Status { get; set; }

    protected Farmer() { }

    public Farmer(Guid id, Guid? tenantId, string code, string fullName, string phoneNumber) : base(id)
    {
        TenantId = tenantId;
        Code = code;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        ReputationScore = 0;
        Status = FarmerStatus.Active;
    }
}
