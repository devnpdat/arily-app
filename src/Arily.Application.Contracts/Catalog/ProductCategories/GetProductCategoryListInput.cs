using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Catalog.ProductCategories;

public class GetProductCategoryListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public CommonStatus? Status { get; set; }
}
