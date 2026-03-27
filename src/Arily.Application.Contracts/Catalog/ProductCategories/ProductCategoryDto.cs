using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Catalog.ProductCategories;

public class ProductCategoryDto : FullAuditedEntityDto<Guid>
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public CommonStatus Status { get; set; }
}
