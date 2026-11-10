using HEJCareSuite.Domain.Entities;
using HEJCareSuite.Application.Mobile;

namespace HEJCareSuite.Infrastructure.Mobile;

public sealed class MockStaffTaskRepository : IStaffTaskRepository
{
    private readonly List<StaffTask> tasks = new();

    public Task AddAsync(StaffTask task, CancellationToken cancellationToken)
    {
        tasks.Add(task);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<StaffTask>> ListByPatientAsync(
        Guid patientId,
        bool? pendingOnly,
        CancellationToken cancellationToken)
    {
        var query = tasks.Where(t => t.PatientId == patientId);
        if (pendingOnly.HasValue && pendingOnly.Value)
            query = query.Where(t => !t.IsCompleted);

        return Task.FromResult<IReadOnlyCollection<StaffTask>>(query.ToList());
    }

    public Task<bool> CompleteAsync(
        Guid taskId,
        Guid patientId,
        CancellationToken cancellationToken)
    {
        var task = tasks.FirstOrDefault(t => t.Id == taskId && t.PatientId == patientId);
        if (task is null)
            return Task.FromResult(false);

        task.Complete();
        return Task.FromResult(true);
    }
}