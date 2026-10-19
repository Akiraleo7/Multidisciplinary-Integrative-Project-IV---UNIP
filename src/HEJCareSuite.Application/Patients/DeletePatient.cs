namespace HEJCareSuite.Application.Patients;

public sealed class DeletePatient(IPatientRepository patientRepository)
{
    public Task<bool> ExecuteAsync(Guid patientId, CancellationToken cancellationToken) =>
        patientRepository.DeleteAsync(patientId, cancellationToken);
}
