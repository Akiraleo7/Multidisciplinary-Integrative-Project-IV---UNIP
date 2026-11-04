using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Application.Hospitalization;

public interface IHospitalAdmissionRepository
{
    Task AddAsync(HospitalAdmission admission, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<HospitalAdmission>> ListByPatientAsync(
        Guid patientId,
        CancellationToken cancellationToken);
    Task<bool> DischargeAsync(
        Guid admissionId,
        Guid patientId,
        string dischargeNotes,
        DateTime dischargedAtUtc,
        CancellationToken cancellationToken);
}
