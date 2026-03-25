namespace Arily.Auditing;

/// <summary>
/// Implement interface này trên AppService để bật log response vào AuditLog.
/// Chỉ có tác dụng khi CustomAuditLogOptions.IsEnabledLogResponse = true.
/// </summary>
public interface IEnableLogResponseAudit
{
}
