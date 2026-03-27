using System.ComponentModel.DataAnnotations;
using Arily.Enums;

namespace Arily.Catalog.ProductCategories;

public class CreateUpdateProductCategoryDto
{
    [Required]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    public CommonStatus Status { get; set; } = CommonStatus.Active;
}
