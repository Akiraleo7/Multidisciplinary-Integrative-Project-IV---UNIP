using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Application.Patients;

public interface IPatientRepository
{
    Task AddAsync(Patient patient, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(
        Guid patientId,
        string fullName,
        DateOnly birthDate,
        string documentNumber,
        CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Patient>> ListAsync(CancellationToken cancellationToken);
}
