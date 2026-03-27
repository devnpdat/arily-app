using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Crm.Customers;

public class CustomerDto : FullAuditedEntityDto<Guid>
{
    public string Code { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? CustomerType { get; set; }
    public string? ProvinceCode { get; set; }
    public string? DistrictCode { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
    public CommonStatus Status { get; set; }
}
