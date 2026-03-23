using System;
using Volo.Abp.Domain.Entities;

namespace Arily.Collection;

/// <summary>Chi tiết đơn thu mua theo grade/loại hàng</summary>
public class PurchaseOrderDetail : Entity<Guid>
{
    public Guid PurchaseOrderId { get; set; }
    public Guid? GradeId { get; set; }
    public string? GradeName { get; set; }

    /// <summary>Số lượng (kg)</summary>
    public decimal QuantityKg { get; set; }

    /// <summary>Đơn giá</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Thành tiền</summary>
    public decimal Amount { get; set; }

    public string? Note { get; set; }

    protected PurchaseOrderDetail() { }

    public PurchaseOrderDetail(Guid id, Guid purchaseOrderId, decimal quantityKg, decimal unitPrice) : base(id)
    {
        PurchaseOrderId = purchaseOrderId;
        QuantityKg = quantityKg;
        UnitPrice = unitPrice;
        Amount = quantityKg * unitPrice;
    }
}
