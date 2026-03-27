using System;
using System.ComponentModel.DataAnnotations;

namespace Arily.Collection.PurchaseAdvances;

public class CreatePurchaseAdvanceDto
{
    [Required]
    public Guid PurchaseOrderId { get; set; }

    [Required]
    public Guid FarmerId { get; set; }

    [Required]
    [Range(0.01, 1000000000)]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = null!;

    [Required]
    public DateTime AdvancedAt { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }
}
