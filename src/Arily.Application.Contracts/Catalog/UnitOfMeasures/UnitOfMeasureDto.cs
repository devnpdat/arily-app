using System;
using Arily.Enums;
using Volo.Abp.Application.Dtos;

namespace Arily.Catalog.UnitOfMeasures;

public class UnitOfMeasureDto : FullAuditedEntityDto<Guid>
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public CommonStatus Status { get; set; }
}
