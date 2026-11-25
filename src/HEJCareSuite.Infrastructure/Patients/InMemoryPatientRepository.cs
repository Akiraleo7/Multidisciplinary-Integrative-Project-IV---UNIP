using System.Collections.Concurrent;
using HEJCareSuite.Application.Patients;
using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Infrastructure.Patients;

public sealed class InMemoryPatientRepository : IPatientRepository
{
    private readonly ConcurrentDictionary<Guid, Patient> patients = new();

    public Task AddAsync(Patient patient, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        patients.TryAdd(patient.Id, patient);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Patient>> ListAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyCollection<Patient>>(patients.Values.ToArray());
    }

    public Task<bool> ExistsWithDocumentAsync(string documentNumber, Guid? excludePatientId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var exists = patients.Values.Any(p =>
            p.DocumentNumber.Equals(documentNumber.Trim(), StringComparison.OrdinalIgnoreCase) &&
            (!excludePatientId.HasValue || p.Id != excludePatientId.Value));

        return Task.FromResult(exists);
    }

    public Task<bool> UpdateAsync(
        Guid patientId,
        string fullName,
        DateOnly birthDate,
        string documentNumber,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!patients.TryGetValue(patientId, out var patient))
            return Task.FromResult(false);

        patient.UpdateDetails(fullName, birthDate, documentNumber);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid patientId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(patients.TryRemove(patientId, out _));
    }
}
