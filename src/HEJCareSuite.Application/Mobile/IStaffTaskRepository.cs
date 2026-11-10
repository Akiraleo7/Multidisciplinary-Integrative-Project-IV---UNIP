using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Application.Mobile;

public interface IStaffTaskRepository
{
    Task AddAsync(StaffTask task, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<StaffTask>> ListByPatientAsync(
        Guid patientId,
        bool? pendingOnly,
        CancellationToken cancellationToken);
    Task<bool> CompleteAsync(
        Guid taskId,
        Guid patientId,
        CancellationToken cancellationToken);
}

public interface ICriticalAlertRepository
{
    Task AddAsync(CriticalAlert alert, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<CriticalAlert>> ListUnacknowledgedAsync(
        Guid? patientId,
        CancellationToken cancellationToken);
    Task<bool> AcknowledgeAsync(
        Guid alertId,
        Guid? acknowledgedByUserId,
        CancellationToken cancellationToken);
}