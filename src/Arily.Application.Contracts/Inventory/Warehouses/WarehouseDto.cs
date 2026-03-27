using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Inventory.Warehouses;

public class WarehouseDto : FullAuditedEntityDto<Guid>
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? ProvinceCode { get; set; }
    public string? Note { get; set; }
    public CommonStatus Status { get; set; }
}
