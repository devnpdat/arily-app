using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Collection.CollectionSessions;

public class GetCollectionSessionListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public CollectionSessionStatus? Status { get; set; }
    public Guid? ProductCategoryId { get; set; }
    public string? RegionProvinceCode { get; set; }
    public DateTime? SessionDateFrom { get; set; }
    public DateTime? SessionDateTo { get; set; }
}
