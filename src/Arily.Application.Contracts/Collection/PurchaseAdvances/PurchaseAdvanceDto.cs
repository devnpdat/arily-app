using System;
using Volo.Abp.Application.Dtos;

namespace Arily.Collection.PurchaseAdvances;

public class PurchaseAdvanceDto : FullAuditedEntityDto<Guid>
{
    public Guid PurchaseOrderId { get; set; }
    public Guid FarmerId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public DateTime AdvancedAt { get; set; }
    public string? Note { get; set; }
}
