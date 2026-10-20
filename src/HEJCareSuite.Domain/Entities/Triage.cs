namespace HEJCareSuite.Domain.Entities;

public enum TriageRiskLevel
{
    Blue = 1,
    Green = 2,
    Yellow = 3,
    Red = 4
}

public sealed class Triage
{
    private Triage()
    {
    }

    public Triage(
        Guid patientId,
        TriageRiskLevel riskLevel,
        string chiefComplaint,
        DateTime recordedAtUtc)
    {
        if (patientId == Guid.Empty)
            throw new ArgumentException("Triage patient is required.", nameof(patientId));
        if (string.IsNullOrWhiteSpace(chiefComplaint))
            throw new ArgumentException("Triage chief complaint is required.", nameof(chiefComplaint));

        Id = Guid.NewGuid();
        PatientId = patientId;
        RiskLevel = riskLevel;
        ChiefComplaint = chiefComplaint.Trim();
        RecordedAtUtc = recordedAtUtc;
    }

    public static Triage Rehydrate(
        Guid id,
        Guid patientId,
        TriageRiskLevel riskLevel,
        string chiefComplaint,
        DateTime recordedAtUtc)
    {
        var triage = new Triage(patientId, riskLevel, chiefComplaint, recordedAtUtc)
        {
            Id = id
        };
        return triage;
    }

    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public TriageRiskLevel RiskLevel { get; private set; }
    public string ChiefComplaint { get; private set; } = string.Empty;
    public DateTime RecordedAtUtc { get; private set; }
}
