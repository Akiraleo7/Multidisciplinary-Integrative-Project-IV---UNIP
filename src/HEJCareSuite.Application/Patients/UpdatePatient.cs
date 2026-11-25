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
        string trimmedDoc = command.DocumentNumber?.Trim() ?? string.Empty;
        if (await patientRepository.ExistsWithDocumentAsync(trimmedDoc, command.PatientId, cancellationToken))
        {
            throw new ArgumentException("A patient with this document number is already registered.", nameof(command.DocumentNumber));
        }

        var validatedPatient = new Domain.Entities.Patient(
            command.FullName,
            command.BirthDate,
            command.DocumentNumber ?? string.Empty);

        return await patientRepository.UpdateAsync(
            command.PatientId,
            validatedPatient.FullName,
            validatedPatient.BirthDate,
            validatedPatient.DocumentNumber,
            cancellationToken);
    }
}
