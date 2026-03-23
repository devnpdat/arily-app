using System.ComponentModel.DataAnnotations;
using Arily.Enums;

namespace Arily.Crm.Farmers;

public class CreateUpdateFarmerDto
{
    [Required]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = null!;

    [StringLength(100)]
    public string? NickName { get; set; }

    [Required]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(20)]
    public string? ProvinceCode { get; set; }

    [StringLength(20)]
    public string? DistrictCode { get; set; }

    [StringLength(20)]
    public string? WardCode { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    public FarmerStatus Status { get; set; } = FarmerStatus.Active;
}
