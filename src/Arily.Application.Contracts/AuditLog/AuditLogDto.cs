using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Arily.AuditLog;

public class AuditLogDto : EntityDto<Guid>
{
    public string? ApplicationName { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string? ClientIpAddress { get; set; }
    public string? HttpMethod { get; set; }
    public string? Url { get; set; }
    public int HttpStatusCode { get; set; }
    public int ExecutionDuration { get; set; }
    public DateTime ExecutionTime { get; set; }
    public string? Exceptions { get; set; }
    public List<AuditLogActionDto> Actions { get; set; } = [];
    public List<EntityChangeDto> EntityChanges { get; set; } = [];
}

public class AuditLogActionDto
{
    public string ServiceName { get; set; } = null!;
    public string MethodName { get; set; } = null!;
    public string? Parameters { get; set; }
    public int ExecutionDuration { get; set; }
    public DateTime ExecutionTime { get; set; }
}

public class EntityChangeDto
{
    public string EntityTypeFullName { get; set; } = null!;
    public string? EntityId { get; set; }
    public byte ChangeType { get; set; } // 0=Created, 1=Updated, 2=Deleted
    public DateTime ChangeTime { get; set; }
}
