using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Sales.CustomerDebtLedgers;

public class CustomerDebtLedgerDto : FullAuditedEntityDto<Guid>
{
    public Guid CustomerId { get; set; }
    public Guid? SalesOrderId { get; set; }
    public CustomerDebtLedgerType LedgerType { get; set; }
    public decimal Amount { get; set; }
    public decimal RunningBalance { get; set; }
    public string? ReferenceNo { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Note { get; set; }
}
