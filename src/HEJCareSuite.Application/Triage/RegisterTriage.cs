using HEJCareSuite.Domain.Entities;
using TriageEntity = HEJCareSuite.Domain.Entities.Triage;

namespace HEJCareSuite.Application.Triage;

public sealed record RegisterTriageCommand(
    Guid PatientId,
    TriageRiskLevel RiskLevel,
    string ChiefComplaint);

public sealed class RegisterTriage(ITriageRepository triageRepository)
{
    public async Task<TriageEntity> ExecuteAsync(
        RegisterTriageCommand command,
        CancellationToken cancellationToken)
    {
        var triage = new TriageEntity(
            command.PatientId,
            command.RiskLevel,
            command.ChiefComplaint,
            DateTime.UtcNow);
        await triageRepository.AddAsync(triage, cancellationToken);
        return triage;
    }
}
