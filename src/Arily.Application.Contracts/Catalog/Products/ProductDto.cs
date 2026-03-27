using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Catalog.Products;

public class ProductDto : FullAuditedEntityDto<Guid>
{
    public Guid ProductCategoryId { get; set; }
    public Guid UnitOfMeasureId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal DefaultLossRate { get; set; }
    public string? Note { get; set; }
    public CommonStatus Status { get; set; }
}
