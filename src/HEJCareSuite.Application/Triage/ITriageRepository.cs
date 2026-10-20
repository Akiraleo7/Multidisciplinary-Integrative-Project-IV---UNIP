using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Application.Triage;

public interface ITriageRepository
{
    Task AddAsync(Triage triage, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Triage>> ListByPatientAsync(
        Guid patientId,
        CancellationToken cancellationToken);
}
