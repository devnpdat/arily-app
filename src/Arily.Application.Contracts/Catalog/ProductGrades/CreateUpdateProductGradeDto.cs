using System.ComponentModel.DataAnnotations;
using Arily.Enums;

namespace Arily.Catalog.ProductGrades;

public class CreateUpdateProductGradeDto
{
    [Required]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    public int SortOrder { get; set; }

    public CommonStatus Status { get; set; } = CommonStatus.Active;
}
