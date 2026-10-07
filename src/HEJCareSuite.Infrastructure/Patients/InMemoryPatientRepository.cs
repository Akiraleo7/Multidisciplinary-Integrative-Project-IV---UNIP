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
}
