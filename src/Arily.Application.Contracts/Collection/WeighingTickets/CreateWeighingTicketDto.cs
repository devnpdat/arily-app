using System;
using System.ComponentModel.DataAnnotations;

namespace Arily.Collection.WeighingTickets;

public class CreateWeighingTicketDto
{
    [Required]
    public Guid PurchaseOrderId { get; set; }

    [Required]
    [StringLength(50)]
    public string TicketNo { get; set; } = null!;

    [Range(0, 100000)]
    public decimal GrossWeightKg { get; set; }

    [Range(0, 100000)]
    public decimal TareWeightKg { get; set; }

    [Required]
    public DateTime WeighedAt { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }
}
