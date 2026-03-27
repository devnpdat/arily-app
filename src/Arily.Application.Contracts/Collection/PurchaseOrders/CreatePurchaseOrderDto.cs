using System;
using System.ComponentModel.DataAnnotations;

namespace Arily.Collection.PurchaseOrders;

public class CreatePurchaseOrderDto
{
    [Required]
    public Guid SessionId { get; set; }

    [Required]
    public Guid FarmerId { get; set; }

    public Guid? GardenId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [StringLength(50)]
    public string OrderNo { get; set; } = null!;

    [Required]
    public DateTime PurchaseDate { get; set; }

    [Range(0, 1000000)]
    public decimal ExpectedQuantityKg { get; set; }

    [Range(0, 10000000)]
    public decimal UnitPrice { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }
}
