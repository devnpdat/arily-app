using System;
using System.ComponentModel.DataAnnotations;
using Arily.Enums;

namespace Arily.Crm.FarmerGardens;

public class CreateUpdateFarmerGardenDto
{
    [Required]
    public Guid ProductCategoryId { get; set; }

    [Required]
    [StringLength(200)]
    public string GardenName { get; set; } = null!;

    [Range(0, 10000)]
    public decimal? AreaHectare { get; set; }

    [Range(0, 10000000)]
    public decimal? EstimatedYieldKg { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    public CommonStatus Status { get; set; } = CommonStatus.Active;
}
