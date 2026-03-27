using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Inventory.Warehouses;

public class GetWarehouseListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public CommonStatus? Status { get; set; }
}
