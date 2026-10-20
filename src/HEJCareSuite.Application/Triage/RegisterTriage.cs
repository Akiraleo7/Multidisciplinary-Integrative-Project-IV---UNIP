using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Application.Triage;

public sealed record RegisterTriageCommand(
    Guid PatientId,
    TriageRiskLevel RiskLevel,
    string ChiefComplaint);

public sealed class RegisterTriage(ITriageRepository triageRepository)
{
    public async Task<Triage> ExecuteAsync(
        RegisterTriageCommand command,
        CancellationToken cancellationToken)
    {
        var triage = new Triage(
            command.PatientId,
            command.RiskLevel,
            command.ChiefComplaint,
            DateTime.UtcNow);
        await triageRepository.AddAsync(triage, cancellationToken);
        return triage;
    }
}
