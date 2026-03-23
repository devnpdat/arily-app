using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Collection;

/// <summary>Phiếu cân</summary>
public class WeighingTicket : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid PurchaseOrderId { get; set; }
    public string TicketNo { get; set; } = null!;

    /// <summary>Cân bì (kg)</summary>
    public decimal GrossWeightKg { get; set; }

    /// <summary>Trọng lượng bao bì (kg)</summary>
    public decimal TareWeightKg { get; set; }

    /// <summary>Trọng lượng thực (kg)</summary>
    public decimal NetWeightKg { get; set; }

    public DateTime WeighedAt { get; set; }
    public DateTime? PrintedAt { get; set; }
    public string? Note { get; set; }

    protected WeighingTicket() { }

    public WeighingTicket(Guid id, Guid? tenantId, Guid purchaseOrderId, string ticketNo, decimal grossWeightKg, decimal tareWeightKg, DateTime weighedAt) : base(id)
    {
        TenantId = tenantId;
        PurchaseOrderId = purchaseOrderId;
        TicketNo = ticketNo;
        GrossWeightKg = grossWeightKg;
        TareWeightKg = tareWeightKg;
        NetWeightKg = grossWeightKg - tareWeightKg;
        WeighedAt = weighedAt;
    }
}
