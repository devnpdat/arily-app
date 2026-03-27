using System.ComponentModel.DataAnnotations;
using Arily.Enums;

namespace Arily.Inventory.Warehouses;

public class CreateUpdateWarehouseDto
{
    [Required]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(20)]
    public string? ProvinceCode { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    public CommonStatus Status { get; set; } = CommonStatus.Active;
}
