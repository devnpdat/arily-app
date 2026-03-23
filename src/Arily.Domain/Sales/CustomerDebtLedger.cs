using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Sales;

/// <summary>Sổ công nợ khách hàng đầu ra</summary>
public class CustomerDebtLedger : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? SalesOrderId { get; set; }
    public CustomerDebtLedgerType LedgerType { get; set; }
    public decimal Amount { get; set; }
    public decimal RunningBalance { get; set; }
    public string? ReferenceNo { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Note { get; set; }

    protected CustomerDebtLedger() { }

    public CustomerDebtLedger(Guid id, Guid? tenantId, Guid customerId, CustomerDebtLedgerType ledgerType, decimal amount, decimal runningBalance, DateTime transactionDate) : base(id)
    {
        TenantId = tenantId;
        CustomerId = customerId;
        LedgerType = ledgerType;
        Amount = amount;
        RunningBalance = runningBalance;
        TransactionDate = transactionDate;
    }
}
