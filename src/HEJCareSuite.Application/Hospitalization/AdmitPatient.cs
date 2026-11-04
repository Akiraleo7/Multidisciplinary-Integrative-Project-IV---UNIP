using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Application.Hospitalization;

public sealed record AdmitPatientCommand(Guid PatientId, string BedNumber);

public sealed class AdmitPatient(IHospitalAdmissionRepository repository)
{
    public async Task<HospitalAdmission> ExecuteAsync(
        AdmitPatientCommand command,
        CancellationToken cancellationToken)
    {
        var admission = new HospitalAdmission(
            command.PatientId,
            command.BedNumber,
            DateTime.UtcNow);
        await repository.AddAsync(admission, cancellationToken);
        return admission;
    }
}
