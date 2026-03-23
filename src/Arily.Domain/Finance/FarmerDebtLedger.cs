using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Finance;

/// <summary>
/// Sổ công nợ nông dân — mỗi phát sinh tạo 1 dòng.
/// Công nợ = Purchase - Advance - LossAdjustment - Payment
/// </summary>
public class FarmerDebtLedger : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid FarmerId { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public FarmerDebtLedgerType LedgerType { get; set; }

    /// <summary>Số tiền phát sinh (dương = nợ tăng, âm = nợ giảm)</summary>
    public decimal Amount { get; set; }

    /// <summary>Số dư công nợ sau phát sinh</summary>
    public decimal RunningBalance { get; set; }

    public string? ReferenceNo { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Note { get; set; }

    protected FarmerDebtLedger() { }

    public FarmerDebtLedger(Guid id, Guid? tenantId, Guid farmerId, FarmerDebtLedgerType ledgerType, decimal amount, decimal runningBalance, DateTime transactionDate) : base(id)
    {
        TenantId = tenantId;
        FarmerId = farmerId;
        LedgerType = ledgerType;
        Amount = amount;
        RunningBalance = runningBalance;
        TransactionDate = transactionDate;
    }
}
