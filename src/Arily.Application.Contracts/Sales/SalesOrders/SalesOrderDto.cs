using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Sales.SalesOrders;

public class SalesOrderDto : FullAuditedEntityDto<Guid>
{
    public Guid? TenantId { get; set; }
    public string OrderNo { get; set; } = null!;
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal NetAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DebtAmount { get; set; }
    public SalesOrderStatus Status { get; set; }
    public string? Note { get; set; }
}
