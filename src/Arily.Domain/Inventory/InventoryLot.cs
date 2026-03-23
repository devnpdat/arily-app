using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Inventory;

/// <summary>Tồn kho theo lô hàng</summary>
public class InventoryLot : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid LotId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? GradeId { get; set; }

    /// <summary>Tồn thực tế (kg)</summary>
    public decimal OnHandQuantityKg { get; set; }

    /// <summary>Đã giữ cho đơn bán (kg)</summary>
    public decimal ReservedQuantityKg { get; set; }

    /// <summary>Có thể bán = OnHand - Reserved</summary>
    public decimal AvailableQuantityKg => OnHandQuantityKg - ReservedQuantityKg;

    public DateTime LastUpdatedAt { get; set; }

    protected InventoryLot() { }

    public InventoryLot(Guid id, Guid? tenantId, Guid warehouseId, Guid lotId, Guid productId, decimal onHandQuantityKg) : base(id)
    {
        TenantId = tenantId;
        WarehouseId = warehouseId;
        LotId = lotId;
        ProductId = productId;
        OnHandQuantityKg = onHandQuantityKg;
        ReservedQuantityKg = 0;
        LastUpdatedAt = DateTime.UtcNow;
    }
}
