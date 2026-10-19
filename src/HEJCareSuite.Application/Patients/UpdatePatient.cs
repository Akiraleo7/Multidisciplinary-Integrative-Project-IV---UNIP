namespace HEJCareSuite.Application.Patients;

public sealed record UpdatePatientCommand(
    Guid PatientId,
    string FullName,
    DateOnly BirthDate,
    string DocumentNumber);

public sealed class UpdatePatient(IPatientRepository patientRepository)
{
    public async Task<bool> ExecuteAsync(
        UpdatePatientCommand command,
        CancellationToken cancellationToken)
    {
        var validatedPatient = new Domain.Entities.Patient(
            command.FullName,
            command.BirthDate,
            command.DocumentNumber);

        return await patientRepository.UpdateAsync(
            command.PatientId,
            validatedPatient.FullName,
            validatedPatient.BirthDate,
            validatedPatient.DocumentNumber,
            cancellationToken);
    }
}
