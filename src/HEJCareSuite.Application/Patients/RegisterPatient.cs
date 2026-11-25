using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Application.Patients;

public sealed record RegisterPatientCommand(
    string FullName,
    DateOnly BirthDate,
    string DocumentNumber);

public sealed class RegisterPatient(IPatientRepository patientRepository)
{
    public async Task<Patient> ExecuteAsync(
        RegisterPatientCommand command,
        CancellationToken cancellationToken)
    {
        string trimmedDoc = command.DocumentNumber?.Trim() ?? string.Empty;
        if (await patientRepository.ExistsWithDocumentAsync(trimmedDoc, null, cancellationToken))
        {
            throw new ArgumentException("A patient with this document number is already registered.", nameof(command.DocumentNumber));
        }

        var patient = new Patient(command.FullName, command.BirthDate, command.DocumentNumber ?? string.Empty);
        await patientRepository.AddAsync(patient, cancellationToken);
        return patient;
    }
}
