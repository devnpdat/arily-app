using System;
using Arily.Enums;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arily.Sales;

/// <summary>Đơn bán hàng cho khách</summary>
public class SalesOrder : FullAuditedAggregateRoot<Guid>, IMultiTenant
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

    /// <summary>Còn nợ = NetAmount - PaidAmount</summary>
    public decimal DebtAmount { get; set; }

    public SalesOrderStatus Status { get; set; }
    public string? Note { get; set; }

    protected SalesOrder() { }

    public SalesOrder(Guid id, Guid? tenantId, string orderNo, Guid customerId, DateTime orderDate) : base(id)
    {
        TenantId = tenantId;
        OrderNo = orderNo;
        CustomerId = customerId;
        OrderDate = orderDate;
        Status = SalesOrderStatus.Draft;
    }
}
