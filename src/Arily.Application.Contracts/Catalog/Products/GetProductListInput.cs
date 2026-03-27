using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Catalog.Products;

public class GetProductListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public CommonStatus? Status { get; set; }
    public Guid? ProductCategoryId { get; set; }
}
