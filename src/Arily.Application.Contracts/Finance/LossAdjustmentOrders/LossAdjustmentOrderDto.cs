using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Finance.LossAdjustmentOrders;

public class LossAdjustmentOrderDto : FullAuditedEntityDto<Guid>
{
    public Guid? TenantId { get; set; }
    public Guid LotId { get; set; }
    public Guid FarmerId { get; set; }
    public Guid PurchaseOrderId { get; set; }
    public string AdjustmentNo { get; set; } = null!;
    public decimal LossQuantityKg { get; set; }
    public decimal LossAmount { get; set; }
    public string? ReasonCode { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public CommonStatus Status { get; set; }
    public string? Note { get; set; }
}
