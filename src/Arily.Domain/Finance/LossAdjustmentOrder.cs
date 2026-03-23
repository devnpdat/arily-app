using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Finance;

/// <summary>Lệnh trừ hao hụt vào công nợ nông dân</summary>
public class LossAdjustmentOrder : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid LotId { get; set; }
    public Guid FarmerId { get; set; }
    public Guid PurchaseOrderId { get; set; }
    public string AdjustmentNo { get; set; } = null!;

    /// <summary>Số lượng hao hụt (kg)</summary>
    public decimal LossQuantityKg { get; set; }

    /// <summary>Số tiền trừ hao hụt</summary>
    public decimal LossAmount { get; set; }

    /// <summary>Lý do: Dập, Hư, Không đạt chuẩn...</summary>
    public string? ReasonCode { get; set; }

    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public CommonStatus Status { get; set; }
    public string? Note { get; set; }

    protected LossAdjustmentOrder() { }

    public LossAdjustmentOrder(Guid id, Guid? tenantId, Guid lotId, Guid farmerId, Guid purchaseOrderId, string adjustmentNo, decimal lossQuantityKg, decimal lossAmount) : base(id)
    {
        TenantId = tenantId;
        LotId = lotId;
        FarmerId = farmerId;
        PurchaseOrderId = purchaseOrderId;
        AdjustmentNo = adjustmentNo;
        LossQuantityKg = lossQuantityKg;
        LossAmount = lossAmount;
        Status = CommonStatus.Active;
    }
}
