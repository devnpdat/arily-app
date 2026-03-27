using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Collection.CollectionSessions;

public class CollectionSessionDto : FullAuditedEntityDto<Guid>
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime SessionDate { get; set; }
    public Guid ProductCategoryId { get; set; }
    public string? RegionProvinceCode { get; set; }
    public string? RegionDistrictCode { get; set; }
    public Guid? BuyerUserId { get; set; }
    public CollectionSessionStatus Status { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? Note { get; set; }
}
