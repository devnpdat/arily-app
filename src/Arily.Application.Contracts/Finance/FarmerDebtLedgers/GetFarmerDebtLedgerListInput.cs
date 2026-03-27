using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Finance.FarmerDebtLedgers;

public class GetFarmerDebtLedgerListInput : PagedAndSortedResultRequestDto
{
    public Guid? FarmerId { get; set; }
    public FarmerDebtLedgerType? LedgerType { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}
