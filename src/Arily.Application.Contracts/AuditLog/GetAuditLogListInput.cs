using System;
using Volo.Abp.Application.Dtos;

namespace Arily.AuditLog;

public class GetAuditLogListInput : PagedAndSortedResultRequestDto
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? UserName { get; set; }
    public string? HttpMethod { get; set; }
    public string? Url { get; set; }
    public int? HttpStatusCode { get; set; }  // ví dụ: 200, 400, 500
    public bool? HasException { get; set; }
}
