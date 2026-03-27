using System;
using Volo.Abp.Application.Dtos;

namespace Arily.Inventory.InventoryLots;

public class InventoryLotDto : FullAuditedEntityDto<Guid>
{
    public Guid WarehouseId { get; set; }
    public Guid LotId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? GradeId { get; set; }
    public decimal OnHandQuantityKg { get; set; }
    public decimal ReservedQuantityKg { get; set; }
    public decimal AvailableQuantityKg { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}
