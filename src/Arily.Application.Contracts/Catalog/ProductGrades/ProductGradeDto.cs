using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Catalog.ProductGrades;

public class ProductGradeDto : FullAuditedEntityDto<Guid>
{
    public Guid ProductId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int SortOrder { get; set; }
    public CommonStatus Status { get; set; }
}
