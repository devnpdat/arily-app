using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Inventory.Lots;

public class GetLotListInput : PagedAndSortedResultRequestDto
{
    public Guid? WarehouseId { get; set; }
    public Guid? ProductId { get; set; }
    public Guid? FarmerId { get; set; }
    public LotStatus? Status { get; set; }
}
