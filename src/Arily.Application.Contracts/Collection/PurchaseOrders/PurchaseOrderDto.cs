using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Collection.PurchaseOrders;

public class PurchaseOrderDto : FullAuditedEntityDto<Guid>
{
    public Guid SessionId { get; set; }
    public Guid FarmerId { get; set; }
    public Guid? GardenId { get; set; }
    public Guid ProductId { get; set; }
    public string OrderNo { get; set; } = null!;
    public DateTime PurchaseDate { get; set; }
    public decimal ExpectedQuantityKg { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal AdvanceAmount { get; set; }
    public decimal LossAmount { get; set; }
    public decimal NetPayableAmount { get; set; }
    public PurchaseOrderStatus Status { get; set; }
    public string? Note { get; set; }
}
