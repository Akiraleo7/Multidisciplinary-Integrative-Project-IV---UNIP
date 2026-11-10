using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Application.Mobile;

public sealed record CreateStaffTaskCommand(
    Guid PatientId,
    string Title,
    string? Description,
    TaskPriority Priority,
    string AssignedRole,
    DateTime? DueDateUtc);

public sealed class CreateStaffTask(IStaffTaskRepository repository)
{
    public async Task<StaffTask> ExecuteAsync(
        CreateStaffTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = new StaffTask(
            command.PatientId,
            command.Title,
            command.Description,
            command.Priority,
            command.AssignedRole,
            command.DueDateUtc);

        await repository.AddAsync(task, cancellationToken);
        return task;
    }
}

public sealed record CompleteStaffTaskCommand(
    Guid TaskId,
    Guid PatientId);

public sealed class CompleteStaffTask(IStaffTaskRepository repository)
{
    public async Task<bool> ExecuteAsync(
        CompleteStaffTaskCommand command,
        CancellationToken cancellationToken)
    {
        return await repository.CompleteAsync(
            command.TaskId,
            command.PatientId,
            cancellationToken);
    }
}