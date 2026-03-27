using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Catalog.UnitOfMeasures;

public class GetUnitOfMeasureListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public CommonStatus? Status { get; set; }
}
