using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Sales.CustomerDebtLedgers;

public class GetCustomerDebtLedgerListInput : PagedAndSortedResultRequestDto
{
    public Guid? CustomerId { get; set; }
    public CustomerDebtLedgerType? LedgerType { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}
