using HEJCareSuite.Domain.Entities;
using HEJCareSuite.Application.Mobile;

namespace HEJCareSuite.Infrastructure.Mobile;

public sealed class MockCriticalAlertRepository : ICriticalAlertRepository
{
    private readonly List<CriticalAlert> alerts = new();

    public Task AddAsync(CriticalAlert alert, CancellationToken cancellationToken)
    {
        alerts.Add(alert);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<CriticalAlert>> ListUnacknowledgedAsync(
        Guid? patientId,
        CancellationToken cancellationToken)
    {
        var query = alerts.Where(a => !a.IsAcknowledged);
        if (patientId.HasValue)
            query = query.Where(a => a.PatientId == patientId.Value || a.PatientId == null);

        return Task.FromResult<IReadOnlyCollection<CriticalAlert>>(query.ToList());
    }

    public Task<bool> AcknowledgeAsync(
        Guid alertId,
        Guid? acknowledgedByUserId,
        CancellationToken cancellationToken)
    {
        var alert = alerts.FirstOrDefault(a => a.Id == alertId && !a.IsAcknowledged);
        if (alert is null)
            return Task.FromResult(false);

        alert.Acknowledge(acknowledgedByUserId);
        return Task.FromResult(true);
    }
}