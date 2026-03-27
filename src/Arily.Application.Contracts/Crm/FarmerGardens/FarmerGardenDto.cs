using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Crm.FarmerGardens;

public class FarmerGardenDto : FullAuditedEntityDto<Guid>
{
    public Guid FarmerId { get; set; }
    public Guid ProductCategoryId { get; set; }
    public string GardenName { get; set; } = null!;
    public decimal? AreaHectare { get; set; }
    public decimal? EstimatedYieldKg { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
    public CommonStatus Status { get; set; }
}
