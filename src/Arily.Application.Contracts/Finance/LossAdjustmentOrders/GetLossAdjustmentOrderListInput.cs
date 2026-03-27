using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Finance.LossAdjustmentOrders;

public class GetLossAdjustmentOrderListInput : PagedAndSortedResultRequestDto
{
    public Guid? FarmerId { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public CommonStatus? Status { get; set; }
}
