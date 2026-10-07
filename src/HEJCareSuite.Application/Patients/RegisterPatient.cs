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
        var patient = new Patient(command.FullName, command.BirthDate, command.DocumentNumber);
        await patientRepository.AddAsync(patient, cancellationToken);
        return patient;
    }
}
