using System;
using System.ComponentModel.DataAnnotations;

namespace Arily.Finance.LossAdjustmentOrders;

public class CreateLossAdjustmentOrderDto
{
    [Required]
    public Guid LotId { get; set; }

    [Required]
    public Guid FarmerId { get; set; }

    [Required]
    public Guid PurchaseOrderId { get; set; }

    [Required]
    [StringLength(50)]
    public string AdjustmentNo { get; set; } = null!;

    [Range(0, 100000)]
    public decimal LossQuantityKg { get; set; }

    [Range(0, 10000000000)]
    public decimal LossAmount { get; set; }

    [StringLength(50)]
    public string? ReasonCode { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }
}
