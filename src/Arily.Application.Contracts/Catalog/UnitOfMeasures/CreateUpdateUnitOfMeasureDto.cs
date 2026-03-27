using System.ComponentModel.DataAnnotations;
using Arily.Enums;

namespace Arily.Catalog.UnitOfMeasures;

public class CreateUpdateUnitOfMeasureDto
{
    [Required]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    public CommonStatus Status { get; set; } = CommonStatus.Active;
}
