using System;
using System.ComponentModel.DataAnnotations;

namespace Arily.Sales.SalesOrders;

public class CreateUpdateSalesOrderDto
{
    [Required]
    [StringLength(50)]
    public string OrderNo { get; set; } = null!;

    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    [Range(0, 100000000000)]
    public decimal TotalAmount { get; set; }

    [Range(0, 100000000000)]
    public decimal DiscountAmount { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }
}
