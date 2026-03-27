using System;
using Volo.Abp.Application.Dtos;

namespace Arily.Collection.WeighingTickets;

public class WeighingTicketDto : FullAuditedEntityDto<Guid>
{
    public Guid PurchaseOrderId { get; set; }
    public string TicketNo { get; set; } = null!;
    public decimal GrossWeightKg { get; set; }
    public decimal TareWeightKg { get; set; }
    public decimal NetWeightKg { get; set; }
    public DateTime WeighedAt { get; set; }
    public DateTime? PrintedAt { get; set; }
    public string? Note { get; set; }
}
