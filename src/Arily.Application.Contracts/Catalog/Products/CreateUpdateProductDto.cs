using System;
using System.ComponentModel.DataAnnotations;
using Arily.Enums;

namespace Arily.Catalog.Products;

public class CreateUpdateProductDto
{
    [Required]
    public Guid ProductCategoryId { get; set; }

    [Required]
    public Guid UnitOfMeasureId { get; set; }

    [Required]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Range(0, 1)]
    public decimal DefaultLossRate { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    public CommonStatus Status { get; set; } = CommonStatus.Active;
}
