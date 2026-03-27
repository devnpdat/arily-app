using System;
using Volo.Abp.Application.Dtos;

namespace Arily.Inventory.InventoryLots;

public class GetInventoryLotListInput : PagedAndSortedResultRequestDto
{
    public Guid? WarehouseId { get; set; }
    public Guid? LotId { get; set; }
    public Guid? ProductId { get; set; }
}
