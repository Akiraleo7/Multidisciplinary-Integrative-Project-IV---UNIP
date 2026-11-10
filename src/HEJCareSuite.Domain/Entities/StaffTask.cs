namespace HEJCareSuite.Domain.Entities;

public enum TaskPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public sealed class StaffTask
{
    private StaffTask() { }

    public StaffTask(
        Guid patientId,
        string title,
        string? description,
        TaskPriority priority,
        string assignedRole,
        DateTime? dueDateUtc)
    {
        if (patientId == Guid.Empty)
            throw new ArgumentException("Patient ID is required.", nameof(patientId));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Task title is required.", nameof(title));

        Id = Guid.NewGuid();
        PatientId = patientId;
        Title = title.Trim();
        Description = description?.Trim();
        Priority = priority;
        AssignedRole = string.IsNullOrWhiteSpace(assignedRole) ? "Nurse" : assignedRole.Trim();
        DueDateUtc = dueDateUtc;
        IsCompleted = false;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public static StaffTask Rehydrate(
        Guid id,
        Guid patientId,
        string title,
        string? description,
        TaskPriority priority,
        bool isCompleted,
        string assignedRole,
        DateTime? dueDateUtc,
        DateTime createdAtUtc,
        DateTime? completedAtUtc)
    {
        return new StaffTask
        {
            Id = id,
            PatientId = patientId,
            Title = title,
            Description = description,
            Priority = priority,
            IsCompleted = isCompleted,
            AssignedRole = assignedRole,
            DueDateUtc = dueDateUtc,
            CreatedAtUtc = createdAtUtc,
            CompletedAtUtc = completedAtUtc
        };
    }

    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TaskPriority Priority { get; private set; }
    public bool IsCompleted { get; private set; }
    public string AssignedRole { get; private set; } = "Nurse";
    public DateTime? DueDateUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }

    public void Complete()
    {
        if (IsCompleted)
            return;

        IsCompleted = true;
        CompletedAtUtc = DateTime.UtcNow;
    }
}
