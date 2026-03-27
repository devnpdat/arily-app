using System;
using System.ComponentModel.DataAnnotations;

namespace Arily.Inventory.Lots;

public class CreateLotDto
{
    [Required]
    [StringLength(100)]
    public string LotCode { get; set; } = null!;

    [Required]
    public Guid PurchaseOrderId { get; set; }

    [Required]
    public Guid FarmerId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public Guid WarehouseId { get; set; }

    [Range(0, 1000000)]
    public decimal ReceivedQuantityKg { get; set; }

    [Required]
    public DateTime ReceivedAt { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }
}
