using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Finance.FarmerDebtLedgers;

public class FarmerDebtLedgerDto : FullAuditedEntityDto<Guid>
{
    public Guid FarmerId { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public FarmerDebtLedgerType LedgerType { get; set; }
    public decimal Amount { get; set; }
    public decimal RunningBalance { get; set; }
    public string? ReferenceNo { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Note { get; set; }
}
