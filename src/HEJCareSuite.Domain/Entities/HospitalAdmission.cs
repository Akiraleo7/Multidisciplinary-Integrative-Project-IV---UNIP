namespace HEJCareSuite.Domain.Entities;

public enum AdmissionStatus
{
    Active = 1,
    Discharged = 2
}

public sealed class HospitalAdmission
{
    private HospitalAdmission()
    {
    }

    public HospitalAdmission(Guid patientId, string bedNumber, DateTime admittedAtUtc)
    {
        if (patientId == Guid.Empty)
            throw new ArgumentException("Patient identifier is required.", nameof(patientId));

        if (string.IsNullOrWhiteSpace(bedNumber))
            throw new ArgumentException("Bed number is required.", nameof(bedNumber));

        Id = Guid.NewGuid();
        PatientId = patientId;
        BedNumber = bedNumber.Trim();
        AdmittedAtUtc = admittedAtUtc;
        Status = AdmissionStatus.Active;
    }

    public static HospitalAdmission Rehydrate(
        Guid id,
        Guid patientId,
        string bedNumber,
        AdmissionStatus status,
        DateTime admittedAtUtc,
        DateTime? dischargedAtUtc,
        string? dischargeNotes)
    {
        var admission = new HospitalAdmission(patientId, bedNumber, admittedAtUtc)
        {
            Id = id,
            Status = status,
            DischargedAtUtc = dischargedAtUtc,
            DischargeNotes = dischargeNotes
        };

        return admission;
    }

    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public string BedNumber { get; private set; } = string.Empty;
    public AdmissionStatus Status { get; private set; }
    public DateTime AdmittedAtUtc { get; private set; }
    public DateTime? DischargedAtUtc { get; private set; }
    public string? DischargeNotes { get; private set; }

    public void Discharge(string dischargeNotes, DateTime dischargedAtUtc)
    {
        if (Status == AdmissionStatus.Discharged)
            throw new InvalidOperationException("The admission has already been discharged.");

        if (string.IsNullOrWhiteSpace(dischargeNotes))
            throw new ArgumentException("Discharge notes are required.", nameof(dischargeNotes));

        Status = AdmissionStatus.Discharged;
        DischargedAtUtc = dischargedAtUtc;
        DischargeNotes = dischargeNotes.Trim();
    }
}
