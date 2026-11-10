using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Application.Mobile;

public sealed record CreateCriticalAlertCommand(
    Guid? PatientId,
    AlertSeverity Severity,
    string Message);

public sealed class CreateCriticalAlert(ICriticalAlertRepository repository)
{
    public async Task<CriticalAlert> ExecuteAsync(
        CreateCriticalAlertCommand command,
        CancellationToken cancellationToken)
    {
        var alert = new CriticalAlert(
            command.PatientId,
            command.Severity,
            command.Message);

        await repository.AddAsync(alert, cancellationToken);
        return alert;
    }
}

public sealed record AcknowledgeCriticalAlertCommand(
    Guid AlertId,
    Guid? AcknowledgedByUserId);

public sealed class AcknowledgeCriticalAlert(ICriticalAlertRepository repository)
{
    public async Task<bool> ExecuteAsync(
        AcknowledgeCriticalAlertCommand command,
        CancellationToken cancellationToken)
    {
        return await repository.AcknowledgeAsync(
            command.AlertId,
            command.AcknowledgedByUserId,
            cancellationToken);
    }
}