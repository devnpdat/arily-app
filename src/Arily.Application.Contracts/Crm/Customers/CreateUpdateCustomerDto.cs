using System.ComponentModel.DataAnnotations;
using Arily.Enums;

namespace Arily.Crm.Customers;

public class CreateUpdateCustomerDto
{
    [Required]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = null!;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(200)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? CustomerType { get; set; }

    [StringLength(20)]
    public string? ProvinceCode { get; set; }

    [StringLength(20)]
    public string? DistrictCode { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    public CommonStatus Status { get; set; } = CommonStatus.Active;
}
