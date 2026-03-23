using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Crm.Farmers;

public class FarmerDto : FullAuditedEntityDto<Guid>
{
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
    public int ReputationScore { get; set; }
    public string? Note { get; set; }
    public FarmerStatus Status { get; set; }
}
