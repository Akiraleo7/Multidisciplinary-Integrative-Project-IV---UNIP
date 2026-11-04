namespace HEJCareSuite.Application.Hospitalization;

public sealed record DischargePatientCommand(
    Guid AdmissionId,
    Guid PatientId,
    string DischargeNotes);

public sealed class DischargePatient(IHospitalAdmissionRepository repository)
{
    public Task<bool> ExecuteAsync(
        DischargePatientCommand command,
        CancellationToken cancellationToken) =>
        repository.DischargeAsync(
            command.AdmissionId,
            command.PatientId,
            command.DischargeNotes,
            DateTime.UtcNow,
            cancellationToken);
}
