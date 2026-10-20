using HEJCareSuite.Domain.Entities;
using TriageEntity = HEJCareSuite.Domain.Entities.Triage;

namespace HEJCareSuite.Application.Triage;

public interface ITriageRepository
{
    Task AddAsync(TriageEntity triage, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TriageEntity>> ListByPatientAsync(
        Guid patientId,
        CancellationToken cancellationToken);
}
