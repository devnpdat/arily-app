using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Sales.SalesOrders;

public class GetSalesOrderListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public Guid? CustomerId { get; set; }
    public SalesOrderStatus? Status { get; set; }
    public DateTime? OrderDateFrom { get; set; }
    public DateTime? OrderDateTo { get; set; }
}
