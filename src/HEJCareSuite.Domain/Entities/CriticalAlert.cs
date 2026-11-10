namespace HEJCareSuite.Domain.Entities;

public enum AlertSeverity
{
    Info = 1,
    Warning = 2,
    Critical = 3,
    Emergency = 4
}

public sealed class CriticalAlert
{
    private CriticalAlert() { }

    public CriticalAlert(
        Guid? patientId,
        AlertSeverity severity,
        string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Alert message is required.", nameof(message));

        Id = Guid.NewGuid();
        PatientId = patientId;
        Severity = severity;
        Message = message.Trim();
        IsAcknowledged = false;
        TriggeredAtUtc = DateTime.UtcNow;
    }

    public static CriticalAlert Rehydrate(
        Guid id,
        Guid? patientId,
        AlertSeverity severity,
        string message,
        bool isAcknowledged,
        DateTime triggeredAtUtc,
        DateTime? acknowledgedAtUtc,
        Guid? acknowledgedByUserId)
    {
        return new CriticalAlert
        {
            Id = id,
            PatientId = patientId,
            Severity = severity,
            Message = message,
            IsAcknowledged = isAcknowledged,
            TriggeredAtUtc = triggeredAtUtc,
            AcknowledgedAtUtc = acknowledgedAtUtc,
            AcknowledgedByUserId = acknowledgedByUserId
        };
    }

    public Guid Id { get; private set; }
    public Guid? PatientId { get; private set; }
    public AlertSeverity Severity { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public bool IsAcknowledged { get; private set; }
    public DateTime TriggeredAtUtc { get; private set; }
    public DateTime? AcknowledgedAtUtc { get; private set; }
    public Guid? AcknowledgedByUserId { get; set; }

    public void Acknowledge(Guid? userId = null)
    {
        IsAcknowledged = true;
        AcknowledgedAtUtc = DateTime.UtcNow;
        if (userId.HasValue)
            AcknowledgedByUserId = userId;
    }
}