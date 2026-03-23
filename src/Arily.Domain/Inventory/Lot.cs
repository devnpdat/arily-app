using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Inventory;

/// <summary>Lô hàng gốc sau thu mua. Ví dụ: SR-TG-2303</summary>
public class Lot : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string LotCode { get; set; } = null!;
    public Guid PurchaseOrderId { get; set; }
    public Guid FarmerId { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }

    /// <summary>Sản lượng nhập lô (kg)</summary>
    public decimal ReceivedQuantityKg { get; set; }

    /// <summary>Sản lượng hiện tại còn lại (kg)</summary>
    public decimal CurrentQuantityKg { get; set; }

    public DateTime ReceivedAt { get; set; }
    public LotStatus Status { get; set; }
    public string? Note { get; set; }

    protected Lot() { }

    public Lot(Guid id, Guid? tenantId, string lotCode, Guid purchaseOrderId, Guid farmerId, Guid productId, Guid warehouseId, decimal receivedQuantityKg, DateTime receivedAt) : base(id)
    {
        TenantId = tenantId;
        LotCode = lotCode;
        PurchaseOrderId = purchaseOrderId;
        FarmerId = farmerId;
        ProductId = productId;
        WarehouseId = warehouseId;
        ReceivedQuantityKg = receivedQuantityKg;
        CurrentQuantityKg = receivedQuantityKg;
        ReceivedAt = receivedAt;
        Status = LotStatus.InStock;
    }
}
