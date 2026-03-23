using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Collection;

/// <summary>Tạm ứng tiền cho nông dân</summary>
public class PurchaseAdvance : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid PurchaseOrderId { get; set; }
    public Guid FarmerId { get; set; }
    public decimal Amount { get; set; }

    /// <summary>Phương thức: Cash, BankTransfer, EWallet</summary>
    public string PaymentMethod { get; set; } = null!;

    public DateTime AdvancedAt { get; set; }
    public string? Note { get; set; }

    protected PurchaseAdvance() { }

    public PurchaseAdvance(Guid id, Guid? tenantId, Guid purchaseOrderId, Guid farmerId, decimal amount, string paymentMethod, DateTime advancedAt) : base(id)
    {
        TenantId = tenantId;
        PurchaseOrderId = purchaseOrderId;
        FarmerId = farmerId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        AdvancedAt = advancedAt;
    }
}
