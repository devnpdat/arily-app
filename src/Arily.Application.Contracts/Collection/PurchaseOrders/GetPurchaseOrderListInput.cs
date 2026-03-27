using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Collection.PurchaseOrders;

public class GetPurchaseOrderListInput : PagedAndSortedResultRequestDto
{
    public Guid? SessionId { get; set; }
    public Guid? FarmerId { get; set; }
    public PurchaseOrderStatus? Status { get; set; }
}
