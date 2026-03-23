using System;
using System.Collections.Generic;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Collection;

/// <summary>Phiếu thu mua từ nông dân</summary>
public class PurchaseOrder : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid SessionId { get; set; }
    public Guid FarmerId { get; set; }
    public Guid? GardenId { get; set; }
    public Guid ProductId { get; set; }
    public string OrderNo { get; set; } = null!;
    public DateTime PurchaseDate { get; set; }

    /// <summary>Sản lượng ước tính (kg)</summary>
    public decimal ExpectedQuantityKg { get; set; }

    /// <summary>Đơn giá (VND/kg)</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Tổng tiền hàng</summary>
    public decimal GrossAmount { get; set; }

    /// <summary>Tổng tạm ứng</summary>
    public decimal AdvanceAmount { get; set; }

    /// <summary>Tiền trừ hao hụt</summary>
    public decimal LossAmount { get; set; }

    /// <summary>Tiền còn phải trả nông dân</summary>
    public decimal NetPayableAmount { get; set; }

    public PurchaseOrderStatus Status { get; set; }
    public string? Note { get; set; }

    public List<PurchaseOrderDetail> Details { get; private set; } = new();

    protected PurchaseOrder() { }

    public PurchaseOrder(Guid id, Guid? tenantId, Guid sessionId, Guid farmerId, Guid productId, string orderNo, DateTime purchaseDate) : base(id)
    {
        TenantId = tenantId;
        SessionId = sessionId;
        FarmerId = farmerId;
        ProductId = productId;
        OrderNo = orderNo;
        PurchaseDate = purchaseDate;
        Status = PurchaseOrderStatus.Draft;
    }
}
