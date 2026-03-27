using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Inventory.Lots;

public class LotDto : FullAuditedEntityDto<Guid>
{
    public string LotCode { get; set; } = null!;
    public Guid PurchaseOrderId { get; set; }
    public Guid FarmerId { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public decimal ReceivedQuantityKg { get; set; }
    public decimal CurrentQuantityKg { get; set; }
    public DateTime ReceivedAt { get; set; }
    public LotStatus Status { get; set; }
    public string? Note { get; set; }
}
