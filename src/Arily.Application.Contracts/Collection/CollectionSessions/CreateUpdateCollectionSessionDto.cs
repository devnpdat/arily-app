using System;
using System.ComponentModel.DataAnnotations;

namespace Arily.Collection.CollectionSessions;

public class CreateUpdateCollectionSessionDto
{
    [Required]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Required]
    public DateTime SessionDate { get; set; }

    [Required]
    public Guid ProductCategoryId { get; set; }

    [StringLength(20)]
    public string? RegionProvinceCode { get; set; }

    [StringLength(20)]
    public string? RegionDistrictCode { get; set; }

    public Guid? BuyerUserId { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }
}
